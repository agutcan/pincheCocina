using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pincheCocina.Data;
using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.Views;
using pincheCocina.MVVM.ViewModels; 

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

            string nombreBaseDatos = "cocina.db3";
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, nombreBaseDatos);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // --- REGISTRO DE VISTAS Y VIEWMODELS ---

            // Registramos el ViewModel
            builder.Services.AddTransient<ListarRecetaViewModel>();

            // Registramos las Vistas
            builder.Services.AddTransient<ListarRecetasView>();
            builder.Services.AddTransient<CrearReceta>();

            // ---------------------------------------

#if WINDOWS
            builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();
#endif

            return builder.Build();
        }
    }
}