using Automaton.Client.Auth.Config;
using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Providers;
using Automaton.Client.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Studio.Config
{
    public static class ConfigurationServiceExtensions
    {
        public static void AddStudioAuthenication<T>(this IServiceCollection services) where T : class, IStorageService
        {
            services.AddScoped<T>();
            services.AddScoped<IStorageService>(sp => sp.GetService<T>());
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            // Services
            services.AddScoped<AuthConfigService>();
            services.AddScoped<RefreshTokenService>();
            services.AddScoped<AuthenticationService>();

            // Models
            services.AddScoped<AuthConfiguration>();
        }
    }
}
