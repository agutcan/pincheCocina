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
        var button = sender as Button;
        var receta = button?.CommandParameter as Receta;

        if (receta == null) return;

        // Pedimos la página al contenedor de servicios
        var page = App.Services.GetRequiredService<CrearReceta>();

        // Le pasamos la receta a editar
        page.RecetaAEditar = receta;

        await Navigation.PushAsync(page);
    }

    private async void OnPasoTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is not PasoReceta paso) return;

        try
        {
            // Usamos StringBuilder para mayor eficiencia si el texto es largo
            var sb = new System.Text.StringBuilder();

            sb.Append($"Paso: {paso.Accion}. ");

            if (paso.TiempoMinutos > 0)
            {
                sb.Append($"Tiempo estimado: {paso.TiempoMinutos} minutos. ");
            }

            if (paso.Ingredientes != null && paso.Ingredientes.Count > 0)
            {
                sb.Append("Ingredientes necesarios: ");
                foreach (var ing in paso.Ingredientes)
                {
                    // Manejamos el reemplazo de forma segura si Unidad es nulo
                    string unidadOriginal = ing.Unidad?.ToLower() ?? "";

                    string unidadLeible = unidadOriginal
                        .Replace("pzas", "piezas")
                        .Replace("pza", "pieza")
                        .Replace("gr", "gramos")
                        .Replace("g", "gramos")
                        .Replace("kg", "kilogramos")
                        .Replace("ml", "mililitros")
                        .Replace("l", "litros");

                    sb.Append($"{ing.Cantidad} {unidadLeible} de {ing.Nombre}. ");
                }
            }

            // Cancelamos cualquier lectura anterior antes de empezar una nueva
            TextToSpeech.Default.SpeakAsync(sb.ToString(), new SpeechOptions
            {
                Pitch = 1.0f,
                Volume = 1.0f
            });
        }
        catch (Exception ex)
        {
            // Si falla el motor de voz (ej. en simuladores), al menos la app no se cierra
            System.Diagnostics.Debug.WriteLine($"Error TTS: {ex.Message}");
        }
    }
}
