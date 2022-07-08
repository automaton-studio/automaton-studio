using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Core.Scripting;

public static class ServiceCollectionExtensions
{
    public static void ConfigureScripting(this IServiceCollection services)
    {
        services.AddSingleton<IScriptEngineFactory, ScriptEngineFactory>();
        services.AddSingleton<IScriptEngineHost, ScriptEngineHost>();
    }
}
