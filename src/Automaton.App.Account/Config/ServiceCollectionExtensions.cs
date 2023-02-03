using Automaton.App.Account;
using Automaton.App.Account.Account;
using Automaton.App.Account.Config;
using Automaton.App.Account.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Studio.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddAccountApp(this IServiceCollection services, IConfiguration configuration)
    {
        // Models
        services.AddScoped<AccountViewModel>();
        services.AddScoped<UserProfileViewModel>();
        services.AddScoped<UserSecurityViewModel>();

        // Services
        services.AddScoped(service => new ConfigurationService(configuration));
        services.AddScoped<UserAccountService>();
    }
}
