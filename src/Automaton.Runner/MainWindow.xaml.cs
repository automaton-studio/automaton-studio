using Automaton.Client.Auth.Interfaces;
using Automaton.Runner.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using Automaton.Client.Auth.Extensions;
using Blazored.LocalStorage;
using System.Configuration;
using Automaton.App.Authentication.Config;

namespace Automaton.Runner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string AppSettings = "appsettings.json";

        public static IConfiguration Configuration { get; private set; }

        public MainWindow()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettings, false, true);

            Configuration = builder.Build();

            var configService = new ConfigService(Configuration);

            var services = new ServiceCollection();

            // Authentication & Authorization
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddStudioAuthenication<LocalStorageService>();

            services.AddWpfBlazorWebView();
            services.AddAntDesign();

            services.AddSingleton(Configuration);
            services.AddSingleton(service => new ConfigService(Configuration));
            services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configService.ApiConfig.BaseUrl) });

            services.AddAuthenticationApp(Configuration);
            services.AddAutomatonCore();

            // Main window
            services.AddTransient(typeof(MainWindow));


            Resources.Add("services", services.BuildServiceProvider());
        }
    }
}
