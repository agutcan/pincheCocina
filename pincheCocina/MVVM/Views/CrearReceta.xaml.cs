using pincheCocina.MVVM.Models;
using pincheCocina.Services;
using System.Globalization;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace pincheCocina.MVVM.Views;

public partial class CrearReceta : ContentPage, INotifyPropertyChanged
{
    private readonly ISpeechToText _speechToText;
    private readonly IRecetaService _recetaService;
    private CancellationTokenSource _tokenSource;

    public Receta RecetaAEditar { get; set; }

    private string _recognitionText;
    public string RecognitionText
    {
        get => _recognitionText;
        set { _recognitionText = value; OnPropertyChanged(nameof(RecognitionText)); }
    }

    private bool _estaEscuchando;
    public bool EstaEscuchando
    {
        get => _estaEscuchando;
        set { _estaEscuchando = value; OnPropertyChanged(nameof(EstaEscuchando)); }
    }

    public CrearReceta(ISpeechToText speechToText, IRecetaService recetaService)
    {
        InitializeComponent();
        _speechToText = speechToText;
        _recetaService = recetaService;
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (RecetaAEditar != null)
        {
            // Modo Edición
            txtNombre.Text = RecetaAEditar.Nombre;
            // Aseguramos que la lista de pasos no sea nula
            if (RecetaAEditar.Pasos == null) RecetaAEditar.Pasos = new List<PasoReceta>();
        }
        else
        {
            // Modo Nuevo
            RecetaAEditar = new Receta { Pasos = new List<PasoReceta>() };
            txtNombre.Text = string.Empty;
        }
    }

    private async void OnListenClicked(object sender, EventArgs e)
    {
        try
        {
            var isAuthorized = await _speechToText.RequestPermissions();
            if (!isAuthorized) return;

            _tokenSource = new CancellationTokenSource();
            EstaEscuchando = true;
            RecognitionText = "Escuchando...";

            var resultado = await _speechToText.Listen(CultureInfo.GetCultureInfo("es-ES"),
                new Progress<string>(p => RecognitionText = p), _tokenSource.Token);

            ProcesarTextoDictado(resultado);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Voz", "Error: " + ex.Message, "OK");
        }
        finally { EstaEscuchando = false; }
    }

    // Ejemplos que funcionan: 
    // Poner a hervir 2 litros de agua luego añadir 500 gramos de macarrones y cocinar por 10 minutos y por último escurrir y servir.
    // Mezclar 250 g de harina con 2 piezas de huevo después añadir 100 g de mantequilla y batir por 5 minutos
    // Añadir 250 ml de leche y 1 l de caldo siguiente paso cocinar a fuego lento por 15 minutos

    private void ProcesarTextoDictado(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return;

        // 1. Normalización (dos -> 2, etc.)
        string textoNormalizado = texto.ToLower()
            .Replace(" dos ", " 2 ")
            .Replace(" un ", " 1 ")
            .Replace(" una ", " 1 ")
            .Replace(" tres ", " 3 ");

        string[] separadoresSilenciosos = { "luego", "después", "siguiente paso", "y por último" };
        string textoMarcado = textoNormalizado;

        foreach (var sep in separadoresSilenciosos)
            textoMarcado = textoMarcado.Replace(sep, "|", StringComparison.OrdinalIgnoreCase);

        textoMarcado = textoMarcado.Replace("añadir", "|añadir", StringComparison.OrdinalIgnoreCase);

        var partes = textoMarcado.Split('|', StringSplitOptions.RemoveEmptyEntries);

        foreach (var parte in partes)
        {
            string pasoLimpio = parte.Trim();
            if (pasoLimpio.Length < 3) continue;

            var nuevoPaso = new PasoReceta
            {
                Accion = pasoLimpio.FirstCharToUpper(),
                Ingredientes = new List<Ingrediente>()
            };

            // 2. Extracción de Tiempo
            var matchTiempo = Regex.Match(pasoLimpio, @"(\d+)\s*(minutos|min|hora|horas)", RegexOptions.IgnoreCase);
            if (matchTiempo.Success)
            {
                int valor = int.Parse(matchTiempo.Groups[1].Value);
                nuevoPaso.TiempoMinutos = matchTiempo.Groups[2].Value.Contains("hora") ? valor * 60 : valor;
            }

            // --- LIMPIEZA DE PALABRAS PEGADAS (Solución Leche1) ---
            string pasoCorregido = Regex.Replace(pasoLimpio, @"([a-zñáéíóú])(\d)", "$1 $2");
            pasoCorregido = pasoCorregido.Replace("con", " con ").Replace("y", " y ").Replace("  ", " ");

            // 3. Extracción de Ingredientes
            string patronIng = @"(\d+(?:[\.,]\d+)?)\s*(gramos|gr|g|kilogramos|kg|ml|litros|l|taza|tazas|piezas|pzas)?\s*(?:de\s+)?([a-záéíóúñ\s]+)";
            var matchesIng = Regex.Matches(pasoCorregido, patronIng, RegexOptions.IgnoreCase);

            foreach (Match m in matchesIng)
            {
                string cantidadStr = m.Groups[1].Value.Replace(',', '.');
                string unidadStr = m.Groups[2].Value.Trim().ToLower();
                string nombreDetectado = m.Groups[3].Value.Trim();

                if (string.IsNullOrEmpty(unidadStr)) unidadStr = "unidad";

                string[] palabrasDeCorte = { " y ", "cocinar", "por", "durante", "hervir", "minutos", "min" };
                foreach (var corte in palabrasDeCorte)
                {
                    int indiceCorte = nombreDetectado.IndexOf(corte, StringComparison.OrdinalIgnoreCase);
                    if (indiceCorte != -1) nombreDetectado = nombreDetectado.Substring(0, indiceCorte).Trim();
                }

                if (!string.IsNullOrWhiteSpace(nombreDetectado) && nombreDetectado.Length > 1)
                {
                    nuevoPaso.Ingredientes.Add(new Ingrediente
                    {
                        Cantidad = double.TryParse(cantidadStr, out double cant) ? cant : 0,
                        Unidad = unidadStr,
                        Nombre = nombreDetectado
                    });
                }
            }
            RecetaAEditar.Pasos.Add(nuevoPaso);
        }
        RecognitionText = $"Receta actualizada: {RecetaAEditar.Pasos.Count} pasos.";
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await DisplayAlert("Aviso", "Por favor, introduce un nombre para la receta.", "OK");
                return;
            }

            RecetaAEditar.Nombre = txtNombre.Text;

            if (RecetaAEditar.Id == 0)
            {
                await _recetaService.AddRecetaAsync(RecetaAEditar);
            }
            else
            {
                await _recetaService.UpdateRecetaAsync(RecetaAEditar);
            }

            // --- NAVEGACIÓN SEGURA ---
            // En lugar de Shell.Current (que te da error), usamos Navigation.PopAsync
            // que es mucho más estable para volver a la pantalla anterior.
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Navigation.PopAsync();
            });
        }
        catch (Exception ex)
        {
            string errorDetallado = ex.Message;
            if (ex.InnerException != null) errorDetallado += " -> " + ex.InnerException.Message;
            await DisplayAlert("Error al Guardar", errorDetallado, "OK");
        }
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        txtNombre.Text = string.Empty;
        RecognitionText = string.Empty;
        RecetaAEditar.Pasos.Clear();
    }

    private void OnCancelListenClicked(object sender, EventArgs e) => _tokenSource?.Cancel();
}

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        string.IsNullOrWhiteSpace(input) ? input : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
}