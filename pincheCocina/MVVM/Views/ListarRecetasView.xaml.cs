using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.ViewModels;

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
}