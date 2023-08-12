using Automaton.Core.Logs;
using Serilog.Events;

namespace Automaton.Studio.Extensions;

public static class EventLogExtensions
{
    public static bool IsWorkflowExecution(this LogEvent logEvent)
    {
        return logEvent.Properties.ContainsKey(LogPropertyKey.Workflow);
    }
}
