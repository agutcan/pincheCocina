using pincheCocina.MVVM.ViewModels;

namespace pincheCocina.MVVM.Views;

public partial class RecetasView : ContentPage
{
    public RecetasView(RecetasViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}

    private void Button_Clicked(object sender, EventArgs e)
    {
        var vm = BindingContext as RecetasViewModel;
        Navigation.PushAsync(new RecetasView(vm));
    }
}