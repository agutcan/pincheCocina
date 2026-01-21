using pincheCocina.MVVM.Views;

namespace pincheCocina
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
        private async void OnGoToListClicked(object sender, EventArgs e)
        {
            // Creamos la instancia de la página manualmente
            // Nota: Como no usas base de datos ahora, podemos hacer "new"
            var paginaListar = new ListarRecetasView();

            // Navegamos usando el Stack de navegación
            await Navigation.PushAsync(paginaListar);
        }
    }
}
