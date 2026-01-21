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
            // EN LUGAR DE: var paginaListar = new ListarRecetasView();
            // USAMOS EL CONTENEDOR DE SERVICIOS:
            var paginaListar = App.Services.GetRequiredService<ListarRecetasView>();

            // Navegamos usando el Stack de navegación
            await Navigation.PushAsync(paginaListar);
        }
    }
}
