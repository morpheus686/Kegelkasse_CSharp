using CommunityToolkit.Maui;
using Kegelkasse.Base.Models;
using Kegelkasse.Base.Services.Interfaces;
using Kegelkasse.Base.ViewModel;
using Kegelkasse.MAUI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using UraniumUI;

namespace Kegelkasse.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder.UseUraniumUI()
            .UseUraniumUIMaterial()// 👈 Don't forget these two lines.
            .UseUraniumUIBlurs()
            .ConfigureMopups();

            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFontAwesomeIconFonts();
                    fonts.AddMaterialSymbolsFonts();
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Directory.GetCurrentDirectory())
            //    .AddJsonFile("appsettings.json", optional: false)
            //    .Build();

            //builder.Services.AddSingleton<IConfiguration>(configuration);
            //var connectionString = configuration.GetConnectionString("SQLiteConnection");

            var dbPath = PrepareDatabase();
            var sqliteConnectionString = $"Data Source={dbPath}";

            builder.Services.AddDbContext<StrafenkatalogContext>(options =>
            {
                options.UseSqlite(sqliteConnectionString);
            });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainWindowViewModel>();
            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<GamePlayerTabViewModel>();
            return builder.Build();
        }


        private static string PrepareDatabase()
        {
            var dbFileName = "Kegelkasse.db";
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, dbFileName);

            if (!File.Exists(dbPath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync(dbFileName).GetAwaiter().GetResult();
                using var newStream = File.Create(dbPath);
                stream.CopyTo(newStream);
            }

            return dbPath;
        }
    }
}
