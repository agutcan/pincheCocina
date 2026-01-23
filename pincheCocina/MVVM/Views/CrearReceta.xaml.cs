using pincheCocina.MVVM.Models;
using pincheCocina.Services;
using System.Globalization;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace pincheCocina.MVVM.Views;

public partial class CrearReceta : ContentPage, INotifyPropertyChanged
{
    private readonly ISpeechToText _speechToText;
    private readonly IRecetaService _recetaService;
    private CancellationTokenSource _tokenSource;

    // Esta es la lista que el XAML observará para pintar los pasos al instante
    public ObservableCollection<PasoReceta> PasosVisuales { get; set; } = new();

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

    private string _modoSeleccionado;
    public string ModoSeleccionado
    {
        get => _modoSeleccionado;
        set { _modoSeleccionado = value; OnPropertyChanged(nameof(ModoSeleccionado)); }
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
            // Modo Edición: Cargamos datos existentes
            txtNombre.Text = RecetaAEditar.Nombre;
            if (RecetaAEditar.Pasos == null) RecetaAEditar.Pasos = new List<PasoReceta>();

            // Sincronizamos la lista visual con los pasos de la receta
            PasosVisuales.Clear();
            foreach (var paso in RecetaAEditar.Pasos)
            {
                PasosVisuales.Add(paso);
            }
        }
        else
        {
            // Modo Nuevo: Limpiamos todo
            RecetaAEditar = new Receta { Pasos = new List<PasoReceta>() };
            txtNombre.Text = string.Empty;
            PasosVisuales.Clear();
        }
    }

    // --- LÓGICA MODO MANUAL ---
    private void OnAnadirPasoManualClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtAccionManual.Text)) return;

        var nuevoPaso = new PasoReceta
        {
            Accion = txtAccionManual.Text.FirstCharToUpper(),
            TiempoMinutos = int.TryParse(txtTiempoManual.Text, out int t) ? t : 0,
            Ingredientes = new List<Ingrediente>()
        };

        // Al añadir a la ObservableCollection, el XAML se actualiza solo sin petar
        PasosVisuales.Add(nuevoPaso);

        txtAccionManual.Text = string.Empty;
        txtTiempoManual.Text = string.Empty;

        lblRecognitionText.Text = $"Pasos en la lista: {PasosVisuales.Count}";
    }

    // --- LÓGICA MODO VOZ ---
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

    private void ProcesarTextoDictado(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return;

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

            var matchTiempo = Regex.Match(pasoLimpio, @"(\d+)\s*(minutos|min|hora|horas)", RegexOptions.IgnoreCase);
            if (matchTiempo.Success)
            {
                int valor = int.Parse(matchTiempo.Groups[1].Value);
                nuevoPaso.TiempoMinutos = matchTiempo.Groups[2].Value.Contains("hora") ? valor * 60 : valor;
            }

            // Sincronizamos con la lista visual
            PasosVisuales.Add(nuevoPaso);
        }
        RecognitionText = $"Voz procesada: {PasosVisuales.Count} pasos en total.";
    }

    // --- GUARDAR Y LIMPIAR ---
    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                await DisplayAlert("Aviso", "Por favor, introduce un nombre.", "OK");
                return;
            }

            // Antes de guardar, pasamos lo que hay en la lista visual a la receta real
            RecetaAEditar.Nombre = txtNombre.Text;
            RecetaAEditar.Pasos = PasosVisuales.ToList();

            if (RecetaAEditar.Id == 0)
                await _recetaService.AddRecetaAsync(RecetaAEditar);
            else
                await _recetaService.UpdateRecetaAsync(RecetaAEditar);

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        txtNombre.Text = string.Empty;
        RecognitionText = string.Empty;
        PasosVisuales.Clear();
    }

    private void OnCancelListenClicked(object sender, EventArgs e) => _tokenSource?.Cancel();

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        string.IsNullOrWhiteSpace(input) ? input : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
}