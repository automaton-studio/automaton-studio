using Automaton.Core.Logs;
using Automaton.Core.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAutomatonCore(this IServiceCollection services)
    {
        services.AddTransient<FlowExecuteService>();
        services.AddScoped<CoreFlowConvertService>();

        return services;
    }
}

