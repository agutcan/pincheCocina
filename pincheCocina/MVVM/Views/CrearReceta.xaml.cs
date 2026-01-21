using pincheCocina.MVVM.Models;
using pincheCocina.Services;
using System.Globalization;
using System.ComponentModel;

namespace pincheCocina.MVVM.Views;

public partial class CrearReceta : ContentPage, INotifyPropertyChanged
{
    private ISpeechToText speechToText;
    private readonly IRecetaService _recetaService;
    private CancellationTokenSource tokenSource;

    // Propiedad para saber si estamos editando
    public Receta RecetaAEditar { get; set; }

    private string recognitionText;
    public string RecognitionText
    {
        get => recognitionText;
        set
        {
            recognitionText = value;
            OnPropertyChanged(nameof(RecognitionText));
        }
    }

    public CrearReceta(ISpeechToText speechToText, IRecetaService recetaService)
    {
        InitializeComponent();
        this.speechToText = speechToText;
        _recetaService = recetaService;

        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Si la propiedad RecetaAEditar tiene datos, es que venimos de "Modificar"
        if (RecetaAEditar != null)
        {
            Title = "Modificar Receta";
            txtNombre.Text = RecetaAEditar.Nombre;
            // Si tuvieras un campo de texto para pasos, aquí lo rellenarías
        }
        else
        {
            Title = "Nueva Receta";
            txtNombre.Text = string.Empty;
            RecognitionText = string.Empty;
        }
    }

    private async void OnGuardarClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text))
        {
            await DisplayAlert("Atención", "Escribe un nombre para la receta", "OK");
            return;
        }

        try
        {
            if (RecetaAEditar == null)
            {
                // MODO CREAR
                var nuevaReceta = new Receta { Nombre = txtNombre.Text };
                await _recetaService.AddRecetaAsync(nuevaReceta);
            }
            else
            {
                // MODO MODIFICAR
                RecetaAEditar.Nombre = txtNombre.Text;
                await _recetaService.UpdateRecetaAsync(RecetaAEditar);
            }

            await Navigation.PopAsync(); // Regresar a la lista
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo guardar: " + ex.Message, "OK");
        }
    }

    private async void OnListenClicked(object sender, EventArgs e)
    {
        var isAuthorized = await speechToText.RequestPermissions();
        if (!isAuthorized)
        {
            await DisplayAlert("Permiso denegado", "No se puede acceder al micrófono", "OK");
            return;
        }

        tokenSource = new CancellationTokenSource();
        RecognitionText = "Escuchando...";

        try
        {
            RecognitionText = await speechToText.Listen(CultureInfo.GetCultureInfo("es-ES"),
                new Progress<string>(partialText =>
                {
                    RecognitionText = partialText;
                }), tokenSource.Token);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }

    private void OnCancelListenClicked(object sender, EventArgs e)
    {
        tokenSource?.Cancel();
    }
}