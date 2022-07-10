using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class Definition
{
    public string Id { get; set; }

    public string Name { get; set; }

    public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

    public TimeSpan? DefaultErrorRetryInterval { get; set; }

    public List<Step> Steps { get; set; } = new List<Step>();

    public Definition()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Untitled";
    }
}
