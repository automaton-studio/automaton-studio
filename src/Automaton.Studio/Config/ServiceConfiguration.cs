using Automaton.Core.Interfaces;
using Automaton.Core.Services;
using Automaton.Studio.AuthProviders;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.Designer;
using Automaton.Studio.Pages.Designer.Components.FlowExplorer;
using Automaton.Studio.Pages.Designer.Components.StepExplorer;
using Automaton.Studio.Pages.Flows;
using Automaton.Studio.Pages.Login;
using Automaton.Studio.Services;
using Automaton.Studio.Services.Interfaces;
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
            services.AddScoped<FlowService>();
            services.AddScoped<FlowConvertService>();
            services.AddScoped<FlowsService>();
            services.AddScoped<NavMenuService>();
            services.AddScoped<IWorkflowExecutor, WorkflowExecutor>();
            services.AddScoped<RefreshTokenService>();
            services.AddScoped<AuthenticationService>();
            services.AddScoped<LocalStorageService>();       

            // ViewModels
            services.AddScoped<FlowsViewModel>();
            services.AddScoped<FlowsViewModel>();
            services.AddScoped<DesignerViewModel>();
            services.AddScoped<StepsViewModel>();
            services.AddScoped<FlowExplorerViewModel>();
            services.AddScoped<LoginViewModel, LoginViewModel>();
            
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
