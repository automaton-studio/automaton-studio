using System;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Steps
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSteps(this IServiceCollection services)
        {
            services.AddTransient<EmitLog>();
        }
    }
}
