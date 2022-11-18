using Automaton.Core.Models;

namespace Automaton.Steps;

public class Test : Sequence
{
    public string? Description { get; set; }

    public IList<string> Errors { get; set; } = new List<string>();

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        return Task.FromResult(ExecutionResult.Next());
    }
}
