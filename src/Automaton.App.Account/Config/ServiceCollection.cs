using Automaton.App.Account.Account;
using Automaton.App.Account.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.App.Account.Config;

public static class ServiceCollection
{
    public static void AddAccountApp(this IServiceCollection services, IConfiguration configuration)
    {
        // Models
        services.AddScoped<AccountViewModel>();
        services.AddScoped<UserProfileViewModel>();
        services.AddScoped<UserSecurityViewModel>();

        // Services
        services.AddScoped<UserAccountService>();
    }
}
