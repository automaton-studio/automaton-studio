using Automaton.Core.Models;
using Automaton.Core.Parsers;
using Newtonsoft.Json.Linq;

namespace Automaton.Steps;

public class TestAssert : WorkflowStep
{
    public string Expression { get; set; }

    public string Error { get; set; }

    /// <summary>
    /// Need to override ExecuteAsync because we do not want Expression to be
    /// evaluated during SetInputProperty in WorkflowStep.
    /// </summary>
    public override async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        foreach (var input in Inputs)
        {
            SetInputProperty(input, context);
        }

        var result = await RunAsync(context);

        return result;
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        var result = ExpressionParser.Parse(Expression, context.Workflow);

        if (result is not bool)
        {
            throw new Exception("Can not evaluate expression to a boolean value");
        }

        if (!(bool)result)
        {
            Error = $"Expression \"{Expression}\" was not true";
        }

        return Task.FromResult(ExecutionResult.Next());
    }

    private void SetInputProperty(KeyValuePair<string, object> input, StepExecutionContext context)
    {
        var stepType = GetType();

        var stepProperty = stepType.GetProperty(input.Key);

        stepProperty.SetValue(this, input.Value);
    }
}
