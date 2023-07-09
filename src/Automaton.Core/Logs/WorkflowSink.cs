using Automaton.Studio.Extensions;
using Serilog.Core;

namespace Automaton.Core.Logs;

public class WorkflowSink : ILogEventSink
{
    public IList<Serilog.Events.LogEvent> LogEvents { get; } = new List<Serilog.Events.LogEvent>();
    public string Logs => string.Join(Environment.NewLine, LogEvents.Select(x => $"{x.Timestamp:s} {x.RenderMessage()}"));

    public void Emit(Serilog.Events.LogEvent logEvent)
    {
        if (logEvent == null) 
            throw new ArgumentNullException(nameof(logEvent));

        if (logEvent.IsWorkflowExecution())
            LogEvents.Add(logEvent);
    }

    public void ClearLogs()
    {
        LogEvents.Clear();
    }
}
