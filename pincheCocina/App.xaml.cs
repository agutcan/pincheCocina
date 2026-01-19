using Microsoft.Extensions.DependencyInjection;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.MVVM.Views;

namespace pincheCocina
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new RecetasView(new RecetasViewModel())));
        }
    }
}