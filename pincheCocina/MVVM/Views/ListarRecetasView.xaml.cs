using pincheCocina.MVVM.ViewModels;

namespace pincheCocina.MVVM.Views;

public partial class ListarRecetasView : ContentPage
{
    public ListarRecetasView()
    {
        InitializeComponent();

        // Asignamos el ViewModel como fuente de datos de la página
        BindingContext = new ListarRecetaViewModel();
    }
}