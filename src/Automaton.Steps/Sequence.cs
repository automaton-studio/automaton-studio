using Automaton.Core.Models;

namespace Automaton.Steps;

public class Sequence : WorkflowStep
{
    public string SequenceEndStepId { get; set; }

    /// <summary>
    /// Need to override ExecuteAsync because we do not want SequenceEndStepId to be
    /// evaluated during SetInputProperty() from parent class WorkflowStep.
    /// </summary>
    public override async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        SequenceEndStepId = Inputs[nameof(SequenceEndStepId)].ToString();

        var result = await RunAsync(context);

        return result;
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }

    protected IEnumerable<WorkflowStep> GetChildren()
    {
        var children = WorkflowDefinition.Steps.Select(x => x.Value).Where(x => x.ParentId == ParentId);

        return children;
    }
}
