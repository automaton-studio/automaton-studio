namespace Automaton.Core.Models;

public class StepExecutionContext
{
    public required Workflow Workflow { get; set; }

    public required WorkflowDefinition Definition { get; set; }

    public required WorkflowStep Step { get; set; }

    public required CancellationToken CancellationToken { get; set; } = CancellationToken.None;
}
