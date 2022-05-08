using Automaton.Runner.Core.Services;
using Automaton.Runner.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Windows;

namespace Automaton.Runner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string AppSettings = "appsettings.json";

        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(AppSettings, false, true);

            Configuration = builder.Build();

            var services = new ServiceCollection();

            services.AddSingleton(Configuration);
            services.AddSingleton(service => new HttpClient
            {
                BaseAddress = new Uri(new ConfigService(Configuration).ApiConfig.BaseUrl)
            });
            services.AddSingleton(service => new ConfigService(Configuration));

            services.AddAutomatonCore();
            services.AddLogging();

            services.AddMediatR(typeof(App));
            services.AddTransient(typeof(MainWindow));

            services.AddApplication();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            ServiceProvider = services.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
