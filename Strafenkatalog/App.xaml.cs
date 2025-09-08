using Kegelkasse;
using Kegelkasse.Models;
using Kegelkasse.Services;
using Kegelkasse.Services.Interfaces;
using Kegelkasse.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace Strafenkatalog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {      
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            services.AddSingleton<IConfiguration>(configuration);
            var connectionString = configuration.GetConnectionString("SQLiteConnection");

            services.AddDbContext<StrafenkatalogContext>(options =>
            {
                options.UseSqlite(connectionString);
            });

            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<GamePlayerTabViewModel>();

            var serviceProvider = services.BuildServiceProvider();
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
