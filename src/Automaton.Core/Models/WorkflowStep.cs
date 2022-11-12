using Automaton.Core.Enums;
using Automaton.Core.Parsers;
using Newtonsoft.Json.Linq;

namespace Automaton.Core.Models;

public abstract class WorkflowStep
{
    public virtual string Id { get; set; }

    public virtual string Name { get; set; }

    public string ParentId { get; set; }

    public string Type { get; set; }

    public string? NextStepId { get; set; }

    public IDictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();

    public IDictionary<string, object> Outputs { get; set; } = new Dictionary<string, object>();

    public virtual WorkflowErrorHandling? ErrorBehavior { get; set; }

    public virtual TimeSpan? RetryInterval { get; set; }

    protected abstract Task<ExecutionResult> RunAsync(StepExecutionContext context);

    public virtual async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        foreach (var input in Inputs)
        {
            SetInputProperty(input, context);
        }

        var result = await RunAsync(context);

        return result;
    }

    public void Setup(Step step)
    {
        Id = step.Id;
        Name = step.Name;
        Type = step.Type;
        NextStepId = step.NextStepId;
        ErrorBehavior = step.ErrorBehavior;
        RetryInterval = step.RetryInterval;
        Inputs = step.Inputs;
    }

    private void SetInputProperty(KeyValuePair<string, object> input, StepExecutionContext context)
    {
        var stepType = GetType();

        var stepProperty = stepType.GetProperty(input.Key);

        var value = ExpressionParser.Parse(input.Value, context.Workflow);

        if (value is JArray array)
        {
            value = array.ToObject(stepProperty.PropertyType);
        }

        if (stepProperty.PropertyType.Name == nameof(Guid))
        {
            Guid.TryParse(value.ToString(), out var guidValue);
            value = guidValue;
        }

        if (stepProperty.PropertyType.Name == nameof(String))
        {
            value = value.ToString();
        }

        stepProperty.SetValue(this, value);
    }
}
