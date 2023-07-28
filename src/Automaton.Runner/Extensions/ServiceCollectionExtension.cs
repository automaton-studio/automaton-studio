using Automaton.Client.Auth.Handlers;
using Automaton.Client.Auth.Http;
using Automaton.Runner.Services;
using Automaton.Runner.Shared;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(service => new ConfigService(configuration));

        //// Scripting
        //services.AddScripting();

        //// Steps
        //services.AddSteps();

        // Services
        //services.AddSingleton<RunnerService>();
        //services.AddSingleton<HubService>();
        //services.AddScoped<FlowService>();
        //services.AddScoped<AuthenticationService>();

        // View models
        services.AddSingleton<MainLayoutViewModel>();

        // Validators
        //services.AddScoped<LoginValidator>();
        //services.AddScoped<RegistrationValidator>();

        // Storage
        services.AddSingleton<ApplicationStorage>();

        services.AddScoped<TokenAuthHeaderHandler>();
        services.AddScoped<AutomatonHttpClient>();
        services.AddHttpClient<AutomatonHttpClient>()
            .AddHttpMessageHandler<TokenAuthHeaderHandler>();
    }
}
