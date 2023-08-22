using Automaton.App.Authentication.Config;
using Automaton.Client.Auth.Extensions;
using Automaton.Client.Auth.Interfaces;
using Automaton.Core.Logs;
using Automaton.Runner.Extensions;
using Automaton.Runner.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Windows;

namespace Automaton.Runner;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const string AppSettings = "appsettings.json";
    public static IConfiguration Configuration { get; private set; }
    public static Services.ConfigurationService ConfigurationService { get; private set; }

    public MainWindow()
    {
        InitializeComponent();

        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(AppSettings, false, true);

        Configuration = builder.Build();
        ConfigurationService = new Services.ConfigurationService(Configuration);

        var services = new ServiceCollection();

        services.AddScoped<JsInterop>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        // Authentication & Authorization
        services.AddBlazoredLocalStorage();

        services.AddScoped<IAuthenticationStorage, DesktopAuthenticationStorage>();

        services.AddAuthorizationCore();
        services.AddStudioAuthentication(Configuration);

        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddAntDesign();
        services.AddScoped<JsInterop>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        services.AddSingleton(Configuration);
        services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(ConfigurationService.BaseUrl) });
        services.AddSingleton(sp => new SerilogHttpClient(new HttpClient { BaseAddress = new Uri(ConfigurationService.BaseUrl) }));

        services.AddAuthenticationApp(Configuration);
        services.AddAutomatonCore();
        services.AddAutomatonSteps();
        services.AddApplication(Configuration);

        services.AddTransient(typeof(MainWindow));

        Resources.Add("services", services.BuildServiceProvider());
    }

    private void OnOpenClick(object sender, RoutedEventArgs e)
    {
        Show();

        WindowState = WindowState.Normal;
    }

    private void OnExitClick(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    /// <summary>
    /// Minimize to system tray when application is minimized.
    /// </summary>
    protected override void OnStateChanged(EventArgs e)
    {
        if (WindowState == WindowState.Minimized) 
            Hide();

        base.OnStateChanged(e);
    }

    /// <summary>
    /// Minimize to system tray when application is closed.
    /// </summary>
    protected override void OnClosing(CancelEventArgs e)
    {
        // setting cancel to true will cancel the close request
        // so the application is not closed
        e.Cancel = true;

        Hide();

        base.OnClosing(e);
    }
}
