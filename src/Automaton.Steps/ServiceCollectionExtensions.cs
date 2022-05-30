using Automaton.Steps;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddSteps(this IServiceCollection services)
    {
        services.AddTransient<EmitLog>();
        services.AddTransient<AddVariable>();
    }
}
