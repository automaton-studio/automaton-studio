using Automaton.Core.Models;

namespace Automaton.Steps;

public class SequenceEnd : WorkflowStep
{
    public string SequenceId { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
