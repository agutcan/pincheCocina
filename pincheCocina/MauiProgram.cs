using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pincheCocina.Data;
using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.Views;
using pincheCocina.MVVM.ViewModels;
using pincheCocina.Services;

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

            // --- CONFIGURACIÓN DE BASE DE DATOS ---
            string nombreBaseDatos = "cocina.db3";
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, nombreBaseDatos);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            // --- REGISTRO DE SERVICIOS (Lógica de negocio) ---
            // Usamos AddSingleton para que solo exista una instancia del servicio en toda la app
            builder.Services.AddSingleton<IRecetaService, RecetaService>();

            // --- REGISTRO DE VISTAS Y VIEWMODELS ---
            // ViewModels
            builder.Services.AddTransient<ListarRecetaViewModel>();
            // Si vas a crear CrearRecetaViewModel más adelante, regístralo aquí:
            // builder.Services.AddTransient<CrearRecetaViewModel>();

            // Vistas
            builder.Services.AddTransient<ListarRecetasView>();
            builder.Services.AddTransient<CrearReceta>();

            // ---------------------------------------

#if DEBUG
            builder.Logging.AddDebug();
#endif

#if WINDOWS
            builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();
#endif

            return builder.Build();
        }
    }
}