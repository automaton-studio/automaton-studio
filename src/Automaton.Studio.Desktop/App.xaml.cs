using Automaton.Studio.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace Automaton.Studio.Desktop;

public partial class App : Application
{
    private const string AppSettings = "appsettings.json";

    public static IServiceProvider ServiceProvider { get; private set; }
    public static IConfiguration Configuration { get; private set; }
    public static IServiceCollection ServiceCollection { get; private set; }

    public static AppState AppState => new();

    protected override void OnStartup(StartupEventArgs e)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSettings, false, true);

        Configuration = builder.Build();
        ServiceCollection = new ServiceCollection();

        ConfigureServices(ServiceCollection);

        ServiceProvider = ServiceCollection.BuildServiceProvider();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Application
        services.AddWpfBlazorWebView();
        services.AddBlazorWebView();
        services.AddAntDesign();
        services.AddStudio(Configuration);

        // Main window
        services.AddSingleton(Configuration);
        services.AddSingleton<AppState>(AppState);
        services.AddTransient(typeof(MainWindow));
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            MessageBox.Show(error.ExceptionObject.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        };
    }
}
