using Microsoft.Extensions.Logging;
using journal2.Services;
using journal2.ViewModels;

namespace journal2;

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

        // ------------------------------
        // Services
        // ------------------------------
        builder.Services.AddSingleton<IDataBaseInitializer, DataBaseInitializer>();
        builder.Services.AddSingleton<IFileExplorer, FileExplorer>();

        // ------------------------------
        // ViewModels
        // ------------------------------
        builder.Services.AddSingleton<MainPageViewModel>();

        // ------------------------------
        // Views / Pages
        // ------------------------------
        builder.Services.AddSingleton<MainPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        // ------------------------------
        // Initialize Database on Startup
        // ------------------------------
        var db = app.Services.GetRequiredService<IDataBaseInitializer>();
        db.DBInitAsync().Wait();   // safe at startup

        return app;
    }
}
