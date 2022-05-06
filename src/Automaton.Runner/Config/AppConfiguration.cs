using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Runner.Validators;
using Automaton.Runner.ViewModels;
using Automaton.Studio.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Config
{
    public static class AppConfiguration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            services.AddSingleton<RegisterService>();
            services.AddSingleton<HubService>();
            services.AddScoped<WorkflowService>();

            // View models
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<LoginViewModel>();
            services.AddSingleton<RegistrationViewModel>();
            services.AddSingleton<DashboardViewModel>();
            services.AddScoped<LoaderViewModel>();

            // Validators
            services.AddScoped<LoginValidator>();
            services.AddScoped<RegistrationValidator>();

            // Other
            services.AddStudioAuthenication<LocalStorageService>();
        }
    }
}
