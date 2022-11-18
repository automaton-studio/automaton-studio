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
        var children = GetChildren();
        var asserts = children.Where(x => x is TestAssert).Select(x => x as TestAssert);

        foreach (var assert in asserts)
        {
            assert.ParentTest = this;
        }

        return Task.FromResult(ExecutionResult.Next());
    }
}
