using Automaton.Core.Models;
using Automaton.Core.Parsers;

namespace Automaton.Steps;

public class TestAssert : WorkflowStep
{
    public string Expression { get; set; }

    public string Error { get; set; }

    public Test ParentTest { get; set; }

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
        logger.Information("""Evaluating expression "{0}" """, Expression);

        var result = ExpressionParser.Parse(Expression, context.Workflow);

        logger.Information("""Expression "{0}" result is {1}""", Expression, result);

        if (result is not bool)
        {
            Error = $"Could not evaluate expression \"{Expression}\" to a boolean value";
            logger.Error("""Could not evaluate expression "{0}" to a boolean value""", Expression);
        }
        else if (!(bool)result)
        {
            Error = $"Expression \"{Expression}\" was not true";
            logger.Error("""Expression "{0}" was not true""", Expression);
        }

        if (ParentTest != null && !string.IsNullOrEmpty(Error))
        {
            ParentTest.AddError(Error);
        }

        return Task.FromResult(ExecutionResult.Next());
    }
}
