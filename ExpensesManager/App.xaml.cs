using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using ExpensesManager.Data;
using ExpensesManager.Interfaces;
using ExpensesManager.Repository;
using ExpensesManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ExpensesManager.View.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ExpensesManager;


/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public static IServiceProvider Services { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();

        var basePath = AppContext.BaseDirectory;
        var dataFolder = Path.Combine(basePath, "Data");
        Directory.CreateDirectory(dataFolder);

        var dbPath = Path.Combine(dataFolder, "app.db");

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<MainWindow>();
        services.AddScoped<MainViewModel>();
        services.AddScoped<SecondWindow>();
        services.AddScoped<AddTransactionViewModel>();
        services.AddScoped<SidePanelViewModel>();

        Services = services.BuildServiceProvider();

        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();

        base.OnStartup(e);
    }
}