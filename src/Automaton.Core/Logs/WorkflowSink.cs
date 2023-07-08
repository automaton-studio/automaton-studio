using Serilog.Core;

namespace Automaton.Core.Logs;

public class WorkflowSink : ILogEventSink
{
    public IList<Serilog.Events.LogEvent> Logs { get; } = new List<Serilog.Events.LogEvent>();

    public void Emit(Serilog.Events.LogEvent logEvent)
    {
        if (logEvent == null) 
            throw new ArgumentNullException(nameof(logEvent));

        Logs.Add(logEvent);
    }
}
