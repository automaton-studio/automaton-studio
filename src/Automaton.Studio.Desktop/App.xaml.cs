using Automaton.Runner.Core.Config;
using Automaton.Studio.Services;
using Automaton.Studio.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;

namespace Automaton.Studio.Desktop
{
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

            var service = new ServiceCollection();
            ConfigureServices(service);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            ServiceProvider = service.BuildServiceProvider();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Application
            services.AddHttpClient();
            services.AddBlazorWebView();
            services.AddAntDesign();
            services.AddSingleton(service => new ConfigService(Configuration));
            services.AddApplication(Configuration);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Main window
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
}
