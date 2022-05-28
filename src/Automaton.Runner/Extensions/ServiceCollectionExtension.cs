using Automaton.Client.Auth.Extensions;
using Automaton.Client.Auth.Handlers;
using Automaton.Client.Auth.Http;
using Automaton.Client.Auth.Interfaces;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Runner.Storage;
using Automaton.Runner.Validators;
using Automaton.Runner.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddSingleton<RunnerService>();
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

        services.AddScoped<TokenAuthHeaderHandler>();
        services.AddScoped<AutomatonHttpClient>();
        services.AddHttpClient<AutomatonHttpClient>()
            .AddHttpMessageHandler<TokenAuthHeaderHandler>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
    }
}
