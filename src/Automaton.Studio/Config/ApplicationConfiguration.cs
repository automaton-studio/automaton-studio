using Automaton.Studio.Services;
using Automaton.Studio.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Automaton.Studio.Config
{
    public static class ConfigurationExtensions
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Services
            services.AddSingleton<IDefinitionService, DefinitionService>();

            // ViewModels
            services.AddScoped<IDefinitionsViewModel, DefinitionsViewModel>();
        }
    }
}
