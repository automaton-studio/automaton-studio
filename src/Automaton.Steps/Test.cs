using Automaton.Core.Models;

namespace Automaton.Steps;

public class Test : Sequence
{
    public IList<string> Errors { get; set; } = new List<string>();

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        SetTestAssertsParent();

        return Task.FromResult(ExecutionResult.Next());
    }

    private void SetTestAssertsParent()
    {
        var testAsserts = GetTestAsserts();

        foreach (var assert in testAsserts)
        {
            assert.ParentTest = this;
        }
    }

    private IEnumerable<TestAssert> GetTestAsserts()
    {
        var children = GetChildren();

        var testAsserts = children.Where(x => x is TestAssert).Select(x => x as TestAssert);

        return testAsserts;
    }
}
