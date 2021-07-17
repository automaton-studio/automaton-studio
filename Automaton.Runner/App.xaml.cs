using Automaton.Runner.Core.Extensions;
using Automaton.Runner.Validators;
using Automaton.Runner.ViewModels;
using Automaton.Runner.ViewModels.Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
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

            var service = new ServiceCollection();
            ConfigureServices(service);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            
            ServiceProvider = service.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private static void ConfigureServices(ServiceCollection services)
        {
            // Application
            services.AddApplication(Configuration);

            // MediateR
            services.AddMediatR(typeof(App));

            // Main window
            services.AddTransient(typeof(MainWindow));

            // View models
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<RegistrationViewModel>();
            services.AddSingleton<DashboardViewModel>();

            // Validators
            services.AddScoped<LoginValidator>();
            services.AddScoped<RegistrationValidator>();

            // Other
            services.AddScoped<IViewModelLoader, ViewModelLoader>();
        }
    }
}
