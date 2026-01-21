<<<<<<< HEAD
<<<<<<< HEAD
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pincheCocina.Data;
=======
﻿using Microsoft.Extensions.Logging;
=======
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pincheCocina.Data;
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198
using pincheCocina.MVVM.Models;
using pincheCocina.MVVM.Views;

#if WINDOWS
using pincheCocina.Platforms;
#endif
<<<<<<< HEAD
>>>>>>> ffdf4cec6159daadb8375bd2f2e66e181a4e5828
=======
>>>>>>> 53d43cf1a168a79dfb7c4ce3f080ca8877dab198

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

            builder.Services.AddTransient<CrearReceta>();

#if WINDOWS

            builder.Services.AddSingleton<ISpeechToText, SpeechToTextImplementation>();
#endif

            return builder.Build();
        }
    }
}
