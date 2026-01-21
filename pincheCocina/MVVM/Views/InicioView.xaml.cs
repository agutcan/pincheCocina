using System.Windows.Input;

namespace pincheCocina.MVVM.Views;

public partial class InicioView : ContentPage
{
    

    public InicioView()
    {
        InitializeComponent();

       
    }

    

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        DisplayAlertAsync("Acción", "Botón MANO presionado", "OK");
    }

    private void ImageButton_Clicked_1(object sender, EventArgs e)
    {
        DisplayAlertAsync("Acción", "Botón MANO presionado", "OK");

    }
}