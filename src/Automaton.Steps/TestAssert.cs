using Automaton.Core.Models;
using Automaton.Core.Parsers;

namespace Automaton.Steps;

public class TestAssert : WorkflowStep
{
    public string Expression { get; set; }

    public string Error { get; set; }

    /// <summary>
    /// Need to override ExecuteAsync because we do not want Expression to be
    /// evaluated during SetInputProperty() from parent class WorkflowStep.
    /// </summary>
    public override async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        Expression = Inputs[nameof(Expression)].ToString();

        var result = await RunAsync(context);

        return result;
    }
     
    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var result = ExpressionParser.Parse(Expression, context.Workflow);

        if (result is not bool)
        {
            Error = $"Could not evaluate expression \"{Expression}\" to a boolean value";
        }
        else if (!(bool)result)
        {
            Error = $"Expression \"{Expression}\" was not true";
        }

        return Task.FromResult(ExecutionResult.Next());
    }
}
