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
            // Resolvemos la vista unificada desde el contenedor de servicios
            // Esto inyectará automáticamente el ViewModel registrado
            var mainPage = Services.GetRequiredService<ListarRecetasView>();

            return new Window(new NavigationPage(mainPage));
        }
    }
}