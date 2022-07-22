using Automaton.Core.Enums;

namespace Automaton.Core.Models;

public class WorkflowDefinition
{
    public string Id { get; set; }

    public IDictionary<string, WorkflowStep> Steps { get; set; } = new Dictionary<string, WorkflowStep>();

    public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

    public TimeSpan? DefaultErrorRetryInterval { get; set; }

    public WorkflowStep GetFirstStep()
    {
        return Steps.First().Value;
    }

    public WorkflowStep? GetNextStep(WorkflowStep step)
    {
        return step.NextStepId != null ? Steps[step.NextStepId] : null;
    }
}
