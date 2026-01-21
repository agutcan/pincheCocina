<<<<<<< HEAD
﻿namespace pincheCocina
=======
﻿using Microsoft.Extensions.DependencyInjection;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.MVVM.Views;

namespace pincheCocina
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
{
    public partial class App : Application
    {

        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
<<<<<<< HEAD

            // Esto define la página de inicio envuelta en navegación clásica
            MainPage = new NavigationPage(new MainPage());
=======
            Services = services;
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
<<<<<<< HEAD
            // IMPORTANTE: Aquí debemos retornar la MainPage que ya definimos
            return new Window(MainPage);
=======
            return new Window(new NavigationPage(new RecetasView(new RecetasViewModel())));
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
        }
    }
}