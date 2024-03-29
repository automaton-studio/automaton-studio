using Automaton.Studio.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace Automaton.Studio.Desktop;

public partial class App : Application
{
    private const string AppSettings = "appsettings.json";

    public static IConfiguration Configuration { get; private set; }
    public static IServiceCollection ServiceCollection { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSettings, false, true);

        Configuration = builder.Build();
        ServiceCollection = new ServiceCollection();

        ServiceCollection.AddWpfBlazorWebView();
        ServiceCollection.AddBlazorWebView();
        ServiceCollection.AddAntDesign();
        ServiceCollection.AddStudio(Configuration);

        ServiceCollection.AddSingleton(Configuration);
        ServiceCollection.AddTransient(typeof(MainWindow));
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
        {
            MessageBox.Show(error.ExceptionObject.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        };
    }
}
