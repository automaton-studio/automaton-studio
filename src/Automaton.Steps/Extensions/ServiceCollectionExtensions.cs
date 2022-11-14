using Automaton.Steps;
using Automaton.Steps.Config;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddSteps(this IServiceCollection services)
    {
        // Note: Must be transient for some reason
        services.AddTransient<EmitLog>();
        services.AddTransient<AddVariable>();
        services.AddTransient<ExecutePython>();
        services.AddTransient<ExecuteFlow>();
        services.AddTransient<Sequence>();
        services.AddTransient<SequenceEnd>();
        services.AddTransient<Test>();
        services.AddTransient<TestAssert>();
        services.AddTransient<TestReport>();

        // Configuration
        services.AddTransient<ConfigService>();  
    }
}
