using Automaton.Runner.Services;
using Serilog.Core;

namespace Automaton.Runner.Logging;

public class ApplicationEnricher : ILogEventEnricher
{
    readonly ConfigurationService configurationService;

    public ApplicationEnricher(ConfigurationService configurationService)
    {
        this.configurationService = configurationService;
    }

    public void Enrich(Serilog.Events.LogEvent logEvent, ILogEventPropertyFactory factory)
    {
        var applicationName = factory.CreateProperty(nameof(configurationService.ApplicationName), configurationService.ApplicationName);
        logEvent.AddPropertyIfAbsent(applicationName);

        var applicationType = factory.CreateProperty(nameof(configurationService.ApplicationType), configurationService.ApplicationType);
        logEvent.AddPropertyIfAbsent(applicationType);

        var runnerId = factory.CreateProperty(nameof(configurationService.RunnerId), configurationService.RunnerId);
        logEvent.AddPropertyIfAbsent(runnerId);
    }
}