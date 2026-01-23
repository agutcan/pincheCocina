using Microsoft.Extensions.Logging;
using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.MVVM.Views;

#if WINDOWS
using pincheCocina.Platforms;
#endif

namespace pincheCocina
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // --- TUS REGISTROS AQUÍ ---

            // 1. Registramos el ViewModel de Recetas
            builder.Services.AddTransient<RecetasViewModel>();

            // 2. Registramos las Vistas (Views)
            builder.Services.AddTransient<InicioView>();
            builder.Services.AddTransient<RecetasView>();
            builder.Services.AddTransient<CrearReceta>();

#if WINDOWS
            builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();
#endif

            return builder.Build();
        }
    }
}
