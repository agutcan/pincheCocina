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
        if (e.Parameter is PasoReceta paso)
        {
            // 1. Construimos el mensaje base
            string mensajeAVoz = $"Paso: {paso.Accion}. ";

            if (paso.TiempoMinutos > 0)
            {
                mensajeAVoz += $"Tiempo estimado: {paso.TiempoMinutos} minutos. ";
            }

            if (paso.Ingredientes != null && paso.Ingredientes.Count > 0)
            {
                mensajeAVoz += "Ingredientes necesarios: ";
                foreach (var ing in paso.Ingredientes)
                {
                    // 2. Aquí hacemos los reemplazos para que las abreviaturas se lean bien
                    // Reemplazamos "pzas" por "piezas" y "gr" por "gramos"
                    string unidadLeible = ing.Unidad.ToLower()
                                            .Replace("pzas", "piezas")
                                            .Replace("pza", "pieza")
                                            .Replace("gr", "gramos")
                                            .Replace("kg", "kilogramos")
                                            .Replace("ml", "mililitros");

                    mensajeAVoz += $"{ing.Cantidad} {unidadLeible} de {ing.Nombre}. ";
                }
            }

            // 3. Ejecutamos la lectura con el texto corregido
            await TextToSpeech.Default.SpeakAsync(mensajeAVoz, new SpeechOptions
            {
                Pitch = 1.0f,
                Volume = 1.0f
            });
        }
    }
}