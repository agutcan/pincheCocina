using pincheCocina.MVVM.ViewModels;
using System.Windows.Input;

namespace pincheCocina.MVVM.Views;

public partial class InicioView : ContentPage
{
    

    public InicioView()
    {
        InitializeComponent();

       
    }

    

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        // 1. Creamos la vista de destino pidiéndola al contenedor
        var page = App.Services.GetRequiredService<RecetasView>();

        // 2. Le pasamos el texto directamente al ViewModel
        if (page.BindingContext is RecetasViewModel vm)
        {
            vm.ModoSeleccionado = "micro";
        }

        // 3. Saltamos a la página
        await Navigation.PushAsync(page);
    }

    private async void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        // 1. Creamos la vista de destino pidiéndola al contenedor
        var page = App.Services.GetRequiredService<RecetasView>();

        // 2. Le pasamos el texto directamente al ViewModel
        if (page.BindingContext is RecetasViewModel vm)
        {
            vm.ModoSeleccionado = "mano";
        }

        // 3. Saltamos a la página
        await Navigation.PushAsync(page);
    }
    
}