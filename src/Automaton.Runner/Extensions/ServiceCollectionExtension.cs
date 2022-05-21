using Automaton.Client.Auth.Extensions;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Runner.Storage;
using Automaton.Runner.Validators;
using Automaton.Runner.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddApplication(this IServiceCollection services)
        {
            // Services
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

            // Storage
            services.AddSingleton<ApplicationStorage>();

            // Other
            services.AddStudioAuthenication<AuthenticationStorage>();
        }
    }
}
