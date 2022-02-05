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
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Services
            services.AddSingleton<IFlowService, FlowService>();
            services.AddSingleton<IFlowsService, FlowsService>();
            services.AddSingleton<INavMenuService, NavMenuService>();

            // ViewModels
            services.AddScoped<FlowsViewModel>();
            services.AddScoped<IFlowViewModel, FlowsViewModel>();
            services.AddScoped<IDesignerViewModel, DesignerViewModel>();
            services.AddScoped<IStepsViewModel, StepsViewModel>();
            services.AddScoped<IFlowExplorerViewModel, FlowExplorerViewModel>();

            // Other
            services.AddSingleton<IStepTypeDescriptor, StepTypeDescriptor>();
            services.AddTransient<StepFactory>();
            services.AddScoped(typeof(DragDropService<>));
        }
    }
}
