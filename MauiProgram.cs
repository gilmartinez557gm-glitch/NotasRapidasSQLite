
using Microsoft.Extensions.Logging;
using NotasRapidasSQLite.Services;
using NotasRapidasSQLite.ViewModels;
using NotasRapidasSQLite.Views;

namespace NotasRapidasSQLite
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

            SQLitePCL.Batteries_V2.Init();

            // Base de datos
            builder.Services.AddSingleton<NotasDatabase>();

            // Navegación principal
            builder.Services.AddSingleton<AppShell>();

            // ViewModels
            builder.Services.AddTransient<NotasViewModel>();
            builder.Services.AddTransient<HistorialViewModel>();
            builder.Services.AddTransient<DetalleNotaViewModel>();

            // Vistas
            builder.Services.AddTransient<NotasPage>();
            builder.Services.AddTransient<HistorialPage>();
            builder.Services.AddTransient<DetalleNotaPage>();

            return builder.Build();
        }
    }
}
