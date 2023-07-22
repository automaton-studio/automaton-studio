using Automaton.App.Authentication.Pages.Login;
using Automaton.App.Authentication.Pages.Register;
using Automaton.App.Authentication.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.App.Authentication.Config;

public static class ServiceCollectionExtensions
{
    public static void AddAuthenticationApp(this IServiceCollection services, IConfiguration configuration)
    {
        // Models
        services.AddScoped<UserRegisterViewModel>();
        services.AddScoped<LoginViewModel>();

        // Services
        services.AddScoped(service => new ConfigurationService(configuration));
        services.AddScoped<AuthenticationService>();
        services.AddScoped<UserRegisterService>();
    }
}
