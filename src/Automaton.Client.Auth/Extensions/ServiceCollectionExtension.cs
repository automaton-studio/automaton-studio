using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using Automaton.Client.Auth.Providers;
using Automaton.Client.Auth.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Client.Auth.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddStudioAuthenication<T>(this IServiceCollection services) where T : class, IAuthenticationStorage
        {
            // Providers
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            // Services
            services.AddScoped<T>();
            services.AddScoped<IAuthenticationStorage>(sp => sp.GetService<T>());
            services.AddScoped<ConfigurationService>();
            services.AddScoped<AuthenticationService>();

            // Models
            services.AddScoped<AuthenticationConfiguration>();
        }
    }
}
