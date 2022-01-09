using Automaton.Studio.Conductor;
using Automaton.Studio.Factories;
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
            services.AddScoped<DefinitionsViewModel>();
            services.AddScoped<IDefinitionsViewModel, DefinitionsViewModel>();
            services.AddScoped<IDesignerViewModel, DesignerViewModel>();
            services.AddScoped<ISolutionStepsViewModel, SolutionStepsViewModel>();
            services.AddSingleton<IStepTypeDescriprot, StepTypeDescriptor>();
            services.AddTransient<StepFactory>();
            services.AddScoped(typeof(DragDropService<>));
        }
    }
}
