using Automaton.Studio.Activities;
using Automaton.Studio.Activity;
using Automaton.Studio.Factories;
using Automaton.Studio.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Automaton.Studio.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private const string ActivitiesAssemblyName = "Automaton.Studio.Activities";

        public static IServiceCollection AddAutomaton(this IServiceCollection services, Action<AutomatonOptionsBuilder>? configure = default)
        {
            // Options
            var optionsBuilder = new AutomatonOptionsBuilder(services);
            
            // Activities
            var assembly = Assembly.Load(ActivitiesAssemblyName);
            optionsBuilder.AddActivitiesFrom(assembly);

            configure?.Invoke(optionsBuilder);

            services.AddTransient(sp => sp.GetRequiredService<AutomatonOptions>());
            services.AddSingleton(optionsBuilder.AutomatonOptions);

            // Factories
            services.AddTransient<ActivityFactory>();

            // Activity descriptors
            services.AddSingleton<IActivityTypeDescriber, ActivityTypeDescriber>();

            return services;
        }
    }
}