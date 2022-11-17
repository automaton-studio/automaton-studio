using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class Step
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public WorkflowErrorHandling? ErrorBehavior { get; set; }

    public TimeSpan? RetryInterval { get; set; }

    public IDictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();

    public IDictionary<string, StepVariable> Outputs { get; set; } = new Dictionary<string, StepVariable>();

    public string? NextStepId { get; set; }

    public string? ParentId { get; set; }
}
