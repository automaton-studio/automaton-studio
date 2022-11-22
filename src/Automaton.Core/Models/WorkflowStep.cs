using Automaton.Core.Enums;
using Automaton.Core.Parsers;
using Newtonsoft.Json.Linq;

namespace Automaton.Core.Models;

public abstract class WorkflowStep
{
    public virtual string Id { get; set; }

    public virtual string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string? ParentId { get; set; }

    public string Type { get; set; }

    public string? NextStepId { get; set; }

    public WorkflowDefinition? WorkflowDefinition { get; set; }

    public IDictionary<string, object> Inputs { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Outputs key is StepVariable.Key while value is StepVariable
    /// </summary>
    public IDictionary<string, StepVariable> Outputs { get; set; } = new Dictionary<string, StepVariable>();

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

    public void SetOutputVariable(string key, object value, Workflow workflow)
    {
        var variable = Outputs[key];
        variable.Value = value;

        workflow.SetVariable(variable);
    }

    public void Setup(Step step, WorkflowDefinition workflowDefinition)
    {
        Id = step.Id;
        Name = step.Name;
        DisplayName = step.DisplayName;
        Description = step.Description;
        Type = step.Type;
        NextStepId = step.NextStepId;
        ParentId = step.ParentId;
        ErrorBehavior = step.ErrorBehavior;
        RetryInterval = step.RetryInterval;
        Inputs = step.Inputs;
        Outputs = step.Outputs;
        WorkflowDefinition = workflowDefinition;
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
