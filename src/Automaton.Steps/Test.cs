using Automaton.Core.Models;

namespace Automaton.Steps;

public class Test : Sequence
{
    public string Description { get; set; }

    public IList<string> Errors { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
