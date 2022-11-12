using Automaton.Core.Models;
using Automaton.Core.Parsers;

namespace Automaton.Steps;

public class TestAssert : WorkflowStep
{
    public string Expression { get; set; }

    public string Error { get; set; }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var result = ExpressionParser.Parse(Expression, context.Workflow);

        if (result is not bool)
        {
            throw new Exception("Can not evaluate expression to a boolean value");
        }

        if (!(bool)result)
        {
            Error = $"{Expression} was not true";
        }

        return Task.FromResult(ExecutionResult.Next());
    }
}
