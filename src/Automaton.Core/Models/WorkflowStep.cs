using Automaton.Core.Enums;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Automaton.Core.Models;

public abstract class WorkflowStep
{
    private const string Percentage = "%";
    private const string VariablePattern = "%.+?%";

    public virtual string Id { get; set; }

    public virtual string Name { get; set; }

    public string Type { get; set; }

    public string? NextStepId { get; set; }

    public virtual List<WorkflowStep> Children { get; set; } = new List<WorkflowStep>();

    public IDictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();

    public IDictionary<string, object> Outputs { get; set; } = new Dictionary<string, object>();

    public virtual WorkflowErrorHandling? ErrorBehavior { get; set; }

    public virtual TimeSpan? RetryInterval { get; set; }

    protected abstract Task<ExecutionResult> RunAsync(StepExecutionContext context);

    public async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        foreach (var input in Inputs)
        {
            Inputs[input.Key] = ParseValue(Inputs[input.Key], context.Workflow);
        }

        var result = await RunAsync(context);

        return result;
    }

    private static object ParseValue(object inputValue, Workflow workflow)
    {
        return ValueHasVariables(inputValue) ? ParseValueWithVariables(inputValue, workflow) : inputValue;
    }

    private static object ParseValueWithVariables(object inputValue, Workflow workflow)
    {
        try
        {
            var variableNames = GetVariableNames(inputValue);
            var parameterExpressions = GetParameterExpressions(variableNames, workflow);

            var sanitizedExpression = inputValue.ToString().Replace(Percentage, string.Empty);
            var lambdaExpresion = DynamicExpressionParser.ParseLambda(parameterExpressions.ToArray(), null, sanitizedExpression);

            var workflowVariables = workflow.GetVariables(variableNames);
            var variableValues = workflowVariables.Select(x => x.Value);

            var value = lambdaExpresion.Compile().DynamicInvoke(variableValues.ToArray());

            return value;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private static bool ValueHasVariables(object inputValue)
    {
        var inputString = inputValue.ToString();
        var regex = new Regex(VariablePattern, RegexOptions.IgnoreCase);
        var matches = regex.Matches(inputString);
        var hasVariables = matches.Any();

        return hasVariables;
    }

    private static IEnumerable<string> GetVariableNames(object inputValue)
    {
        var inputString = inputValue.ToString();
        var regex = new Regex(VariablePattern, RegexOptions.IgnoreCase);
        var matches = regex.Matches(inputString);
        var variableNames = matches.Select(x => x.Value.Replace(Percentage, string.Empty));

        return variableNames;
    }

    private static IEnumerable<ParameterExpression> GetParameterExpressions(IEnumerable<string> variableNames, Workflow workflow)
    {
        var variableExpressions = new List<ParameterExpression>();

        foreach (var name in variableNames)
        {
            var variable = workflow.GetVariable(name);
            var variableExpression = Expression.Parameter(typeof(string), variable.Key);
            variableExpressions.Add(variableExpression);
        }

        return variableExpressions;
    }
}
