using AutoMapper;
using Automaton.Client.Auth.Extensions;
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
using Automaton.Studio.Steps.AddVariable;
using Automaton.Studio.Steps.EmitLog;
using Automaton.Studio.Steps.ExecutePython;
using Automaton.Studio.Steps.ExecuteFlow;
using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Automaton.Studio.Mapper;
using Automaton.Studio.Steps.Test;
using Automaton.Studio.Steps.Sequence;
using Automaton.Studio.Steps.TestAssert;
using Automaton.Studio.Steps.TestReport;
using Microsoft.Extensions.Logging;
using Automaton.Studio.Logging;
using Microsoft.Extensions.Options;

namespace Automaton.Studio.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddStudio(this IServiceCollection services, IConfiguration configuration)
    {
        var configService = new ConfigurationService(configuration);

        // Automaton Core
        services.AddAutomatonCore();

        // Scripting
        services.AddScripting();

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
        services.AddScoped<ErrorService>();

        // ViewModels
        services.AddScoped<FlowsViewModel>();
        services.AddScoped<FlowsViewModel>();
        services.AddScoped<DesignerViewModel>();
        services.AddScoped<StepsViewModel>();
        services.AddScoped<FlowExplorerViewModel>();
        services.AddScoped<LoginViewModel>();
        services.AddScoped<AccountViewModel>();

        // Steps
        services.AddScoped<IStepTypeDescriptor, StepTypeDescriptor>();
        services.AddScoped<StepFactory>();
        services.AddSteps();

        // Studio steps
        // Note: Must be transient for some reason
        services.AddTransient<EmitLogStep>();
        services.AddTransient<AddVariableStep>();
        services.AddTransient<ExecutePythonStep>();
        services.AddTransient<ExecuteFlowStep>();
        services.AddTransient<TestStep>();
        services.AddTransient<TestAssertStep>();
        services.AddTransient<TestReportStep>();
        services.AddTransient<SequenceStep>();
        services.AddTransient<SequenceEndStep>();

        // Models
        services.AddScoped<AppConfig>();
        services.AddScoped<ApiConfig>();

        // Javascript
        services.AddScoped<JsInterop>();

        // Other
        services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configService.BaseUrl) });
        services.AddScoped(typeof(DragDropService));
        services.AddScoped(service => new ConfigurationService(configuration));

        services.AddSingleton<ILoggerProvider, ApplicationLoggerProvider>(services =>
        {
            var httpClient = services.GetService<HttpClient>();
            return new ApplicationLoggerProvider(httpClient, new ConfigurationService(configuration));
        });

        // Automapper profile
        services.AddScoped(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile(provider.GetService<StepFactory>()));
        }).CreateMapper());
    }
}
