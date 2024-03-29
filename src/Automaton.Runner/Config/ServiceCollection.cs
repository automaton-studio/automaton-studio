﻿using Automaton.Runner.Pages.Dashboard;
using Automaton.Runner.Pages.Settings;
using Automaton.Runner.Pages.Setup;
using Automaton.Runner.Services;
using Automaton.Runner.Shared;
using Automaton.Runner.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Config;

public static class ServiceCollection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(service => new ConfigurationService(configuration));
        services.AddSingleton<ApplicationStorage>();
        services.AddSingleton<HubService>();
        services.AddSingleton<RunnerService>();
        services.AddScoped<FlowService>();
        services.AddScoped<FlowExecutionsService>();
        services.AddScoped<RunnerFlowExecuteService>();

        // View models
        services.AddSingleton<MainLayoutViewModel>();
        services.AddSingleton<SetupViewModel>();
        services.AddSingleton<RunnerAppViewModel>();
        services.AddSingleton<DashboardViewModel>();
        services.AddSingleton<SettingsViewModel>();
    }
}
