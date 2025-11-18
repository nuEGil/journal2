using Microsoft.Extensions.Logging;
using journal2.Services;
using journal2.ViewModels;

namespace journal2
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

            // ------------------------------
            // Services
            // ------------------------------
            builder.Services.AddSingleton<IDataBaseInitializer, DataBaseInitializer>();
            builder.Services.AddSingleton<IFileExplorer, FileExplorer>();
            builder.Services.AddSingleton<IKeywordService, KeywordService>();
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();
            builder.Services.AddSingleton<IFileSeeder, FileSeeder>();

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
            return builder.Build();
        }
    }
}