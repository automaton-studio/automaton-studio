using Automaton.Runner.Core;
using Automaton.Runner.Validators;
using Automaton.Runner.ViewModels;
using Automaton.Runner.ViewModels.Common;
using FluentValidation;
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
            services.AddInfrastructure(Configuration);
            services.AddApplication(Configuration);

            // MediateR dependency injection
            services.AddMediatR(typeof(App));

            // Register main window
            services.AddTransient(typeof(MainWindow));

            // Register all ViewModels.
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<RegistrationViewModel>();
            services.AddSingleton<DashboardViewModel>();

            // Register validators
            services.AddScoped<LoginValidator>();

            // Register common
            services.AddScoped<IViewModelLoader, ViewModelLoader>();
        }
    }
}
