﻿using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Core.Scripting;

public static class ServiceCollectionExtensions
{
    public static void AddScripting(this IServiceCollection services)
    {
        services.AddSingleton<ScriptEngineFactory>();
        services.AddSingleton<ScriptEngineHost>();
    }
}