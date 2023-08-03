using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Providers;
using Automaton.Client.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Client.Auth.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddStudioAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Providers
        services.AddScoped<AuthStateProvider>();
        services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

        // Services
        services.AddScoped(service => new ClientAuthConfigurationService(configuration));
        services.AddScoped<AuthTokenService>();      

        // Models
        services.AddScoped<ClientAuthConfig>();
    }
}
