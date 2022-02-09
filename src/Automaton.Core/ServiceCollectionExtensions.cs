using Automaton.Core.Interfaces;
using Automaton.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkflow(this IServiceCollection services)
        {
            services.AddTransient<IStepExecutor, StepExecutor>();
            services.AddTransient<IWorkflowExecutor, WorkflowExecutor>();
            
            return services;
        }
    }
}

