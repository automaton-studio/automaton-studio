using Automaton.Core.Models;
using Microsoft.Scripting.Utils;

namespace Automaton.Steps;

public class Test : Sequence
{
    public string Description { get; set; }

    public IList<string> Errors { get; set; } = new List<string>();

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var asserts = GetTestAsserts();

        Errors.AddRange(asserts.Select(x => x.Error));

        return Task.FromResult(ExecutionResult.Next());
    }

    private IEnumerable<TestAssert> GetTestAsserts()
    {
        var children = GetChildren();

        var testAsserts = children.Where(x => x is TestAssert).Select(x => x as TestAssert);

        return testAsserts;
    }
}
