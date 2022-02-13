using Automaton.Core.Interfaces;
using Automaton.Core.Services;
using Automaton.Studio.Components.Explorer.FlowExplorer;
using Automaton.Studio.Components.Explorer.StepExplorer;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Factories;
using Automaton.Studio.Services;
using Automaton.Studio.Services.Interfaces;
using Automaton.Studio.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Automaton.Studio.Config
{
    public static class ConfigurationExtensions
    {
        public static void AddStudio(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Services
            services.AddSingleton<IFlowService, FlowService>();
            services.AddSingleton<IFlowConvertService, FlowConvertService>();
            services.AddSingleton<IFlowsService, FlowsService>();
            services.AddSingleton<INavMenuService, NavMenuService>();
            services.AddSingleton<IWorkflowExecutor, WorkflowExecutor>();

            // ViewModels
            services.AddScoped<FlowsViewModel>();
            services.AddScoped<IFlowViewModel, FlowsViewModel>();
            services.AddScoped<IDesignerViewModel, DesignerViewModel>();
            services.AddScoped<IStepsViewModel, StepsViewModel>();
            services.AddScoped<IFlowExplorerViewModel, FlowExplorerViewModel>();

            // Steps
            services.AddSteps();

            // Other
            services.AddSingleton<IStepTypeDescriptor, StepTypeDescriptor>();
            services.AddTransient<StepFactory>();
            services.AddScoped(typeof(DragDropService<>));
        }
    }
}
