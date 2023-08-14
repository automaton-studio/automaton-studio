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
        var applicatioName = factory.CreateProperty(nameof(configurationService.ApplicationName), configurationService.ApplicationName);
        logEvent.AddPropertyIfAbsent(applicatioName);

        var applicationType = factory.CreateProperty(nameof(configurationService.ApplicationType), configurationService.ApplicationType);
        logEvent.AddPropertyIfAbsent(applicationType);
    }
}