using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Studio.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Automaton.Runner.Core.Extensions
{
    public static class ServiceConfiguration
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var configService = new ConfigService(configuration);

            services.AddSingleton(configuration);
            services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configService.StudioConfig.WebApiUrl) });

            services.AddSingleton(service => new ConfigService(configuration));
            services.AddSingleton<RegisterService>();
            services.AddSingleton<HubService>();
            services.AddScoped<WorkflowService>();

            services.AddAutomatonCore();
            services.AddStudioAuthenication<LocalStorageService>();
        }
    }
}
