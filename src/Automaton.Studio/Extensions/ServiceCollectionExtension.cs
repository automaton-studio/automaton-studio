using Automaton.Client.Auth.Extensions;
using Automaton.Client.Auth.Interfaces;
using Automaton.Core.Scripting;
using Automaton.Studio.Config;
using Automaton.Studio.Domain;
using Automaton.Studio.Domain.Interfaces;
using Automaton.Studio.Factories;
using Automaton.Studio.Pages.Account;
using Automaton.Studio.Pages.Designer;
using Automaton.Studio.Pages.Designer.Components.FlowExplorer;
using Automaton.Studio.Pages.Designer.Components.StepExplorer;
using Automaton.Studio.Pages.Flows;
using Automaton.Studio.Pages.Login;
using Automaton.Studio.Services;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;

namespace Automaton.Studio.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddStudio(this IServiceCollection services, IConfiguration configuration)
        {
            var configService = new ConfigurationService(configuration);

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Automaton Core
            services.AddAutomatonCore();

            // Scripting
            services.ConfigureScripting();

            // Authentication & Authorization
            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddStudioAuthenication<LocalStorageService>();
            services.AddScoped<AuthenticationService>();

            // Services
            services.AddScoped<FlowService>();
            services.AddScoped<FlowsService>();
            services.AddScoped<RunnerService>();
            services.AddScoped<LocalStorageService>();
            services.AddSingleton<NavMenuService>();

            // ViewModels
            services.AddScoped<FlowsViewModel>();
            services.AddScoped<FlowsViewModel>();
            services.AddScoped<DesignerViewModel>();
            services.AddScoped<StepsViewModel>();
            services.AddScoped<FlowExplorerViewModel>();
            services.AddScoped<LoginViewModel>();
            services.AddScoped<AccountViewModel>();      

            // Steps
            services.AddSingleton<IStepTypeDescriptor, StepTypeDescriptor>();
            services.AddTransient<StepFactory>();
            services.AddSteps();

            // Models
            services.AddScoped<AppConfiguration>();
            services.AddScoped<ApiConfiguration>();

            // Other
            services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(configService.BaseUrl) });
            services.AddScoped(typeof(DragDropService<>));
            services.AddScoped(service => new ConfigurationService(configuration));
        }
    }
}
