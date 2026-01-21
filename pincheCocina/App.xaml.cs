using Microsoft.Extensions.DependencyInjection;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.MVVM.Views;

namespace pincheCocina
{
    public partial class App : Application
    {

        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();

            Services = services;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // IMPORTANTE: Aquí debemos retornar la MainPage que ya definimos
            return new Window(new NavigationPage(new RecetasView(new RecetasViewModel())));
        }
    }
}