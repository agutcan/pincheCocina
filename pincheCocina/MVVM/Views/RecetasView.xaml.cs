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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var page = App.Services.GetRequiredService<CrearReceta>();
        await Navigation.PushAsync(page);
    }
}