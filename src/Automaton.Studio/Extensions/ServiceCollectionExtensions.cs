using AutoMapper;
using Automaton.App.Account.Config;
using Automaton.App.Authentication.Config;
using Automaton.Client.Auth.Extensions;
using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Services;
using Automaton.Core.Logs;
using Automaton.Core.Scripting;
using Automaton.Studio.Config;
using Automaton.Studio.Domain;
using Automaton.Studio.Factories;
using Automaton.Studio.Mapper;
using Automaton.Studio.Pages.CustomStepDesigner;
using Automaton.Studio.Pages.CustomSteps;
using Automaton.Studio.Pages.FlowDesigner;
using Automaton.Studio.Pages.FlowDesigner.Components.FlowExplorer;
using Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;
using Automaton.Studio.Pages.Flows;
using Automaton.Studio.Pages.Runners;
using Automaton.Studio.Services;
using Automaton.Studio.Shared;
using Automaton.Studio.Steps.AddVariable;
using Automaton.Studio.Steps.EmitLog;
using Automaton.Studio.Steps.ExecuteFlow;
using Automaton.Studio.Steps.ExecutePython;
using Automaton.Studio.Steps.If;
using Automaton.Studio.Steps.Sequence;
using Automaton.Studio.Steps.Test;
using Automaton.Studio.Steps.TestAssert;
using Automaton.Studio.Steps.TestReport;
using Blazored.LocalStorage;
using MediatR.Courier;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;

namespace Automaton.Studio.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddStudio(this IServiceCollection services, IConfiguration configuration)
    {
        var configService = new Services.ConfigurationService(configuration);

        services.AddAccountApp(configuration);
        services.AddAuthenticationApp(configuration);
  
        services.AddAutomatonCore();
        services.AddAutomatonSteps();
        services.AddScoped<StepTypeDescriptor>();
        services.AddScoped<StepFactory>();

        // Scripting
        services.AddScripting();

        // Authentication & Authorization
        services.AddBlazoredLocalStorage();
        services.AddAuthorizationCore();
        services.AddStudioAuthentication(configuration);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddCourier(Assembly.GetExecutingAssembly());
        services.AddScoped<IAuthenticationStorage, WebAuthenticationStorage>();

        // Services
        services.AddScoped<Services.ConfigurationService>();
        services.AddScoped<UserAccountService>();      
        services.AddScoped<FlowService>();
        services.AddScoped<FlowsService>();
        services.AddScoped<RunnerService>();
        services.AddScoped<ErrorService>();
        services.AddScoped<FlowExecutionsService>();
        services.AddScoped<CustomStepsService>();
        services.AddScoped<StudioFlowExecuteService>();

        // ViewModels
        services.AddScoped<FlowsViewModel>();
        services.AddScoped<RunnersViewModel>();
        services.AddScoped<CustomStepsViewModel>();
        services.AddScoped<DesignerViewModel>();
        services.AddScoped<CustomStepViewModel>();     
        services.AddScoped<StepsViewModel>();
        services.AddScoped<FlowExplorerViewModel>();
        services.AddScoped<MainLayoutViewModel>(); 

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
        services.AddTransient<IfStep>();
        services.AddTransient<Steps.Custom.CustomStep>();

        // Models
        services.AddScoped<AppConfig>();
        services.AddScoped<ApiConfig>();

        // Javascript
        services.AddScoped<JsInterop>();

        // Other
        services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configService.BaseUrl) });      
        services.AddSingleton(sp => new SerilogHttpClient(new HttpClient { BaseAddress = new Uri(configService.BaseUrl) }));
        services.AddSingleton(sp => new WorkflowLogsSink());

        services.AddScoped(typeof(DragDropService));

        services.AddLogging(x =>
        {
            x.ClearProviders();
            x.AddSerilog(dispose: true);
        });

#if DEBUG
        Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
        Serilog.Debugging.SelfLog.Enable(Console.Error);
#endif

        services.AddScoped(provider => new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoMapperProfile(provider));
        }).CreateMapper());
    }
}
