using Automaton.Runner.Core.Services;
using Automaton.Runner.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;

namespace Automaton.Runner;

public partial class App : Application
{
    private const string AppSettings = "appsettings.json";
    private const string AutomatonIsolatedStorage = "Automaton";

    public static IServiceProvider ServiceProvider { get; private set; }
    public static IConfiguration Configuration { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        RestoreApplicationProperties();

        ServiceProvider = BuildServiceProvider();

        var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();

        mainWindow.Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        PersistApplicationProperties();
    }

    private static IServiceProvider BuildServiceProvider()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSettings, false, true);

        Configuration = builder.Build();

        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("Automaton.log")
            .CreateLogger();

        var services = new ServiceCollection();

        services.AddSingleton(Configuration);

        services.AddSingleton(service => new ConfigService(Configuration));

        services.AddAutomatonCore();

        services.AddLogging(configure => configure.AddSerilog())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information)
            .AddTransient<App>();

        services.AddMediatR(typeof(App));
        services.AddTransient(typeof(MainWindow));

        services.AddApplication();

        var serviceProvider = services.BuildServiceProvider();

        return serviceProvider;
    }

    private static void PersistApplicationProperties()
    {
        var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();

        using var writer = new StreamWriter(new IsolatedStorageFileStream(AutomatonIsolatedStorage, FileMode.Create, isolatedStorage));

        writer.Write(JsonConvert.SerializeObject(App.Current.Properties));
    }

    private static void RestoreApplicationProperties()
    {
        var isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            
        using var reader = new StreamReader(new IsolatedStorageFileStream(AutomatonIsolatedStorage, FileMode.OpenOrCreate, isolatedStorage));

        if (reader != null)
        {
            var propertiesString = reader.ReadToEnd();

            if (string.IsNullOrEmpty(propertiesString))
                return;

            var properties = JsonConvert.DeserializeObject<Dictionary<string, object>>(propertiesString);

            foreach(var property in properties)
            {
                App.Current.Properties[property.Key] = property.Value;
            }
        }     
    }
}
