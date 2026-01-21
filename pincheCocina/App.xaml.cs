namespace pincheCocina
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Esto define la página de inicio envuelta en navegación clásica
            MainPage = new NavigationPage(new MainPage());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // IMPORTANTE: Aquí debemos retornar la MainPage que ya definimos
            return new Window(MainPage);
        }
    }
}