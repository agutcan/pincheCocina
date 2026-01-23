using pincheCocina.MVVM.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace pincheCocina.MVVM.Views;

public partial class RecetasView : ContentPage
{

    public RecetasView(RecetasViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var vm = (RecetasViewModel)BindingContext;

        if (vm.ModoSeleccionado == "micro")
        {
            // Aquí pones lo que quieres que pase con el Micro
            await DisplayAlertAsync("Aviso", "Has pulsado el MICRO", "OK");
        }
        else if (vm.ModoSeleccionado == "mano")
        {
            // Aquí pones lo que quieres que pase con la Mano
            await DisplayAlertAsync("Aviso", "Has pulsado la MANO", "OK");
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var page = App.Services.GetRequiredService<CrearReceta>();
        await Navigation.PushAsync(page);
    }
}