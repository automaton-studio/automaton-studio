using Automaton.Core.Models;

namespace Automaton.Steps;

public class Sequence : WorkflowStep
{
    public string SequenceEndId { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
