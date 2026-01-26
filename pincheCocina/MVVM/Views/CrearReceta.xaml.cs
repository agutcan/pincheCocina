using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.Services;
using System.Globalization;

namespace pincheCocina.MVVM.Views;

public partial class CrearReceta : ContentPage
{
    private readonly ISpeechToText _speechToText;
    private readonly CrearRecetaViewModel _viewModel;
    private CancellationTokenSource _tokenSource;

    // Propiedades puente para mantener compatibilidad con tu navegación actual
    public Receta RecetaAEditar { get; set; }
    public string ModoSeleccionado { set => _viewModel.ModoSeleccionado = value; }

    public CrearReceta(ISpeechToText speechToText, IRecetaService recetaService)
    {
        InitializeComponent();
        _speechToText = speechToText;
        _viewModel = new CrearRecetaViewModel(recetaService);
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.CargarReceta(RecetaAEditar);
    }

    private void OnAnadirPasoManualClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtAccionManual.Text)) return;

        var nuevoPaso = new PasoReceta
        {
            Accion = txtAccionManual.Text.FirstCharToUpper(),
            TiempoMinutos = int.TryParse(txtTiempoManual.Text, out int t) ? t : 0,
            Ingredientes = new List<Ingrediente>()
        };

        _viewModel.PasosVisuales.Add(nuevoPaso);
        txtAccionManual.Text = string.Empty;
        txtTiempoManual.Text = string.Empty;
    }

    private async void OnListenClicked(object sender, EventArgs e)
    {
        try
        {
            var isAuthorized = await _speechToText.RequestPermissions();
            if (!isAuthorized) return;

            _tokenSource = new CancellationTokenSource();
            _viewModel.EstaEscuchando = true;
            _viewModel.RecognitionText = "Escuchando...";

            var resultado = await _speechToText.Listen(CultureInfo.GetCultureInfo("es-ES"),
                new Progress<string>(p => _viewModel.RecognitionText = p), _tokenSource.Token);

            _viewModel.ProcesarTextoDictado(resultado);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Voz", "Error: " + ex.Message, "OK");
        }
        finally { _viewModel.EstaEscuchando = false; }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        bool guardado = await _viewModel.GuardarReceta();
        if (guardado)
            await Navigation.PopAsync();
        else
            await DisplayAlert("Aviso", "Introduce un nombre para la receta", "OK");
    }

    private void OnLimpiarClicked(object sender, EventArgs e)
    {
        _viewModel.NombreReceta = string.Empty;
        _viewModel.RecognitionText = string.Empty;
        _viewModel.PasosVisuales.Clear();
    }

    private void OnCancelListenClicked(object sender, EventArgs e) => _tokenSource?.Cancel();
}
