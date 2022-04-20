using Automaton.Core.Interfaces;
using Automaton.Core.Services;
using Automaton.Studio.AuthProviders;
using Automaton.Studio.Components.Explorer.FlowExplorer;
using Automaton.Studio.Components.Explorer.StepExplorer;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.Login;
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

            // Authentication & Authorization
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(configService.WebApiUrl) });

            // Services
            services.AddScoped<IFlowService, FlowService>();
            services.AddScoped<IFlowConvertService, FlowConvertService>();
            services.AddScoped<IFlowsService, FlowsService>();
            services.AddScoped<INavMenuService, NavMenuService>();
            services.AddScoped<IWorkflowExecutor, WorkflowExecutor>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IStorageService, LocalStorageService>();       

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
            services.AddScoped(service => new ConfigService(configuration));
        }
    }
}
