using Automaton.Core.Models;

namespace Automaton.Steps;

public class Sequence : WorkflowStep
{
    public string? SequenceEndStepId { get; set; }

    /// <summary>
    /// Need to override ExecuteAsync because we do not want SequenceEndStepId to be
    /// evaluated during SetInputProperty() from parent class WorkflowStep.
    /// </summary>
    public override async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        SequenceEndStepId = Inputs[nameof(SequenceEndStepId)].Value.ToString();

        var result = await RunAsync(context);

        return result;
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }

    protected IEnumerable<WorkflowStep> GetChildren()
    {
        var children = new List<WorkflowStep>();

        var nextStepId = NextStepId;

        while (nextStepId != SequenceEndStepId)
        {
            var child = WorkflowDefinition.Steps[nextStepId];

            children.Add(child);

            nextStepId = child.NextStepId;
        }

        return children;
    }
}
