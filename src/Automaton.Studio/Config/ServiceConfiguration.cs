using Automaton.Core.Interfaces;
using Automaton.Core.Services;
using Automaton.Studio.AuthProviders;
using Automaton.Studio.Components.Explorer.FlowExplorer;
using Automaton.Studio.Components.Explorer.StepExplorer;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Factories;
using Automaton.Studio.Services;
using Automaton.Studio.Services.Interfaces;
using Automaton.Studio.ViewModels;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;

namespace Automaton.Studio.Config
{
    public static class ConfigurationExtensions
    {
        public static void AddStudio(this IServiceCollection services, IConfiguration configuration)
        {
            var configService = new ConfigService(configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configService.WebApiUrl) });

            // Authentication & Authorization
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

            // Services
            services.AddSingleton<IFlowService, FlowService>();
            services.AddSingleton<IFlowConvertService, FlowConvertService>();
            services.AddSingleton<IFlowsService, FlowsService>();
            services.AddSingleton<INavMenuService, NavMenuService>();
            services.AddSingleton<IWorkflowExecutor, WorkflowExecutor>();
            services.AddSingleton<ILoginService, LoginService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();          

            // ViewModels
            services.AddScoped<FlowsViewModel>();
            services.AddScoped<IFlowViewModel, FlowsViewModel>();
            services.AddScoped<IDesignerViewModel, DesignerViewModel>();
            services.AddScoped<IStepsViewModel, StepsViewModel>();
            services.AddScoped<IFlowExplorerViewModel, FlowExplorerViewModel>();
            services.AddScoped<ILoginViewModel, LoginViewModel>();
            
            // Steps
            services.AddSingleton<IStepTypeDescriptor, StepTypeDescriptor>();
            services.AddTransient<StepFactory>();
            services.AddSteps();

            // Other
            services.AddScoped(typeof(DragDropService<>));
        }
    }
}
