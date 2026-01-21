using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pincheCocina.Data;

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
            // 1. Definir el nombre del archivo de la base de datos
            string nombreBaseDatos = "cocina.db3";

            // 2. Crear la ruta completa usando la carpeta de datos de la app
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, nombreBaseDatos);

            // 3. Registrar el DbContext con la ruta generada
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
