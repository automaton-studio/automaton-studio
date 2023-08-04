using Automaton.Client.Auth.Handlers;
using Automaton.Client.Auth.Http;
using Automaton.Runner.Pages.Setup;
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
        services.AddSingleton<ApplicationStorage>();
        services.AddSingleton<HubService>();
        services.AddSingleton<RunnerService>();

        //// Scripting
        //services.AddScripting();

        //// Steps
        //services.AddSteps();

        // View models
        services.AddSingleton<MainLayoutViewModel>();
        services.AddSingleton<SetupViewModel>();  

        // Validators
        //services.AddScoped<LoginValidator>();
        //services.AddScoped<RegistrationValidator>();

        services.AddScoped<TokenAuthHeaderHandler>();
        services.AddScoped<AutomatonHttpClient>();
        services.AddHttpClient<AutomatonHttpClient>()
            .AddHttpMessageHandler<TokenAuthHeaderHandler>();
    }
}
