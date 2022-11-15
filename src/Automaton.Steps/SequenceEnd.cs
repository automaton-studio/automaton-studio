using Automaton.Core.Models;

namespace Automaton.Steps;

public class SequenceEnd : WorkflowStep
{
    public string SequenceStepId { get; set; }

    /// <summary>
    /// Need to override ExecuteAsync because we do not want SequenceStepId to be
    /// evaluated during SetInputProperty() from parent class WorkflowStep.
    /// </summary>
    public override async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        SequenceStepId = Inputs[nameof(SequenceStepId)].ToString();

        var result = await RunAsync(context);

        return result;
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
