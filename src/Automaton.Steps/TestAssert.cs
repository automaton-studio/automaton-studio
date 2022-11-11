using Automaton.Core.Models;

namespace Automaton.Steps;

public class TestAssert : WorkflowStep
{
    public string Message { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
