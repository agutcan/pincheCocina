using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.ViewModels;
using Microsoft.Maui.Media;

namespace pincheCocina.MVVM.Views;

public partial class ListarRecetasView : ContentPage
{
    private readonly ListarRecetaViewModel _viewModel;

    public ListarRecetasView(ListarRecetaViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarRecetasAsync();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var page = App.Services.GetRequiredService<CrearReceta>();
        page.ModoSeleccionado = _viewModel.ModoSeleccionado;
        await Navigation.PushAsync(page);
    }

    private async void OnEliminarClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var receta = button?.CommandParameter as Receta;

        if (receta == null) return;

        bool answer = await DisplayAlert("Eliminar Receta", $"¿Estás seguro de que quieres borrar '{receta.Nombre}'?", "Sí", "No");

        if (answer)
        {
            await _viewModel.EliminarRecetaAsync(receta.Id);
            await _viewModel.CargarRecetasAsync();
        }
    }

    private async void OnModificarClicked(object sender, EventArgs e)
    {
        try
        {
            var button = sender as Button;
            var receta = button?.CommandParameter as Receta;

            if (receta == null) return;

            var page = App.Services.GetRequiredService<CrearReceta>();
            page.ModoSeleccionado = _viewModel.ModoSeleccionado;
            page.RecetaAEditar = receta;

            await Navigation.PushAsync(page);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "No se pudo abrir la edición: " + ex.Message, "OK");
        }
    }

    private async void OnPasoTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not PasoReceta paso) return;

        try
        {
            // El ViewModel construye el string
            string textoParaHablar = _viewModel.ObtenerTextoLecturaPaso(paso);

            // La View ejecuta la acción de voz
            await TextToSpeech.Default.SpeakAsync(textoParaHablar, new SpeechOptions
            {
                Pitch = 1.0f,
                Volume = 1.0f
            });
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error TTS: {ex.Message}");
        }
    }
}