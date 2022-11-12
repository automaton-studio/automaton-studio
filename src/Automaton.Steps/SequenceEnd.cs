using Automaton.Core.Models;

namespace Automaton.Steps;

public class SequenceEnd : WorkflowStep
{
    public string SequenceStepId { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
