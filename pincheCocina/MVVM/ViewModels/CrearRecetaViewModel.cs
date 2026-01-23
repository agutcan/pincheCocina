using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.Views;
using pincheCocina.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace pincheCocina.MVVM.ViewModels;

public class CrearRecetaViewModel : INotifyPropertyChanged
{
    private readonly IRecetaService _recetaService;

    public ObservableCollection<PasoReceta> PasosVisuales { get; set; } = new();
    public Receta RecetaAEditar { get; set; }

    private string _recognitionText;
    public string RecognitionText
    {
        get => _recognitionText;
        set { _recognitionText = value; OnPropertyChanged(); }
    }

    private bool _estaEscuchando;
    public bool EstaEscuchando
    {
        get => _estaEscuchando;
        set { _estaEscuchando = value; OnPropertyChanged(); }
    }

    private string _modoSeleccionado;
    public string ModoSeleccionado
    {
        get => _modoSeleccionado;
        set { _modoSeleccionado = value; OnPropertyChanged(); }
    }

    private string _nombreReceta;
    public string NombreReceta
    {
        get => _nombreReceta;
        set { _nombreReceta = value; OnPropertyChanged(); }
    }

    public CrearRecetaViewModel(IRecetaService recetaService)
    {
        _recetaService = recetaService;
    }

    public void CargarReceta(Receta receta)
    {
        RecetaAEditar = receta ?? new Receta { Pasos = new List<PasoReceta>() };
        NombreReceta = RecetaAEditar.Nombre;
        PasosVisuales.Clear();

        if (RecetaAEditar.Pasos != null)
        {
            foreach (var paso in RecetaAEditar.Pasos) PasosVisuales.Add(paso);
        }
    }

    // Ejemplos que funcionan: 
    // Poner a hervir 2 litros de agua luego añadir 500 gramos de macarrones y cocinar por 10 minutos y por último escurrir y servir.
    // Mezclar 250 g de harina con 2 piezas de huevo después añadir 100 g de mantequilla y batir por 5 minutos
    // Añadir 250 ml de leche y 1 l de caldo siguiente paso cocinar a fuego lento por 15 minutos
    // --- LÓGICA MODO VOZ COMPLETA ---
    public void ProcesarTextoDictado(string texto)
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
            // Separa letras de números si el dictado los pegó
            string pasoCorregido = Regex.Replace(pasoLimpio, @"([a-zñáéíóú])(\d)", "$1 $2");
            pasoCorregido = pasoCorregido.Replace("con", " con ").Replace("y", " y ").Replace("  ", " ");

            // 3. Extracción de Ingredientes
            // Regex para capturar: [Cantidad] [Unidad opcional] [Nombre del ingrediente]
            string patronIng = @"(\d+(?:[\.,]\d+)?)\s*(gramos|gr|g|kilogramos|kg|ml|litros|l|taza|tazas|piezas|pzas)?\s*(?:de\s+)?([a-záéíóúñ\s]+)";
            var matchesIng = Regex.Matches(pasoCorregido, patronIng, RegexOptions.IgnoreCase);

            foreach (Match m in matchesIng)
            {
                string cantidadStr = m.Groups[1].Value.Replace(',', '.');
                string unidadStr = m.Groups[2].Value.Trim().ToLower();
                string nombreDetectado = m.Groups[3].Value.Trim();

                if (string.IsNullOrEmpty(unidadStr)) unidadStr = "unidad";

                // Limpieza: Evitamos que el nombre del ingrediente se coma verbos o conectores
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

            // Sincronizamos con la lista que ve el usuario
            PasosVisuales.Add(nuevoPaso);
        }

        RecognitionText = $"Receta actualizada: {PasosVisuales.Count} pasos.";
    }

    public async Task<bool> GuardarReceta()
    {
        if (string.IsNullOrWhiteSpace(NombreReceta)) return false;

        RecetaAEditar.Nombre = NombreReceta;
        RecetaAEditar.Pasos = PasosVisuales.ToList();

        if (RecetaAEditar.Id == 0)
            await _recetaService.AddRecetaAsync(RecetaAEditar);
        else
            await _recetaService.UpdateRecetaAsync(RecetaAEditar);

        return true;
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        string.IsNullOrWhiteSpace(input) ? input : string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1));
}