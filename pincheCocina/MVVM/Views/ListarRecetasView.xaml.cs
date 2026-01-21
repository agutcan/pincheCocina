using pincheCocina.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace pincheCocina.MVVM.Views;

public partial class ListarRecetasView : ContentPage
{
    // Recibimos el ViewModel por Inyección de Dependencias
    public ListarRecetasView(ListarRecetaViewModel vm)
    {
        InitializeComponent();

        // Asignamos el ViewModel unificado
        BindingContext = vm;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        // Usamos la lógica de navegación que tenías en la primera vista
        // Esto asume que tienes configurado App.Services en tu App.xaml.cs
        var page = App.Services.GetRequiredService<CrearReceta>();
        await Navigation.PushAsync(page);
    }
}