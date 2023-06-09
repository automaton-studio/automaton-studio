namespace Automaton.Core.Models;

public class WorkflowDefinition
{
    public string Id { get; set; }

    public IDictionary<string, WorkflowStep> Steps { get; set; } = new Dictionary<string, WorkflowStep>();

    public WorkflowStep GetFirstStep()
    {
        return Steps.First().Value;
    }
}
