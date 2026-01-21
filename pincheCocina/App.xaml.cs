using Microsoft.Extensions.DependencyInjection;
using pincheCocina.MVVM.Views;

namespace pincheCocina
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new InicioView());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(MainPage);
        }
    }
}