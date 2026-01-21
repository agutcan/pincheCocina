<<<<<<< HEAD
<<<<<<< HEAD
﻿namespace pincheCocina
=======
=======
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
﻿using Microsoft.Extensions.DependencyInjection;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.MVVM.Views;

namespace pincheCocina
<<<<<<< HEAD
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
=======
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
{
    public partial class App : Application
    {

        public static IServiceProvider Services { get; private set; }

        public App(IServiceProvider services)
        {
            InitializeComponent();
<<<<<<< HEAD
<<<<<<< HEAD

            // Esto define la página de inicio envuelta en navegación clásica
            MainPage = new NavigationPage(new MainPage());
=======
            Services = services;
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
=======

            Services = services;
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            // IMPORTANTE: Aquí debemos retornar la MainPage que ya definimos
            return new Window(MainPage);
=======
            return new Window(new NavigationPage(new RecetasView(new RecetasViewModel())));
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
=======
            // IMPORTANTE: Aquí debemos retornar la MainPage que ya definimos
            return new Window(new NavigationPage(new RecetasView(new RecetasViewModel())));
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
        }
    }
}