﻿using Automaton.Core.Attributes;
using Automaton.Core.Extensions;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Automaton.Core.Models;

public abstract class WorkflowStep
{
    protected readonly ILogger logger;

    public virtual string Id { get; set; }

    public virtual string Name { get; set; }

    public string DisplayName { get; set; }

    public string Description { get; set; }

    public string? ParentId { get; set; }

    public string Type { get; set; }

    public string? NextStepId { get; set; }

    public WorkflowDefinition? WorkflowDefinition { get; set; }

    public IDictionary<string, StepVariable> Inputs { get; set; } = new Dictionary<string, StepVariable>();

    public IDictionary<string, StepVariable> Outputs { get; set; } = new Dictionary<string, StepVariable>();

    protected abstract Task<ExecutionResult> RunAsync(StepExecutionContext context);

    public WorkflowStep()
    {
        logger = Log.ForContext<WorkflowStep>();
    }

    public void Setup(Step step, WorkflowDefinition workflowDefinition)
    {
        // Need to deep clone the incoming step to avoid Inputs and Outputs
        // being updated during execution of the flow and then passed back to designer
        var stepClone = step.CloneJson();

        Id = stepClone.Id;
        Name = stepClone.Name;
        DisplayName = stepClone.DisplayName;
        Description = stepClone.Description;
        Type = stepClone.Type;
        NextStepId = stepClone.NextStepId;
        ParentId = stepClone.ParentId;
        Inputs = stepClone.Inputs;
        Outputs = stepClone.Outputs;
        WorkflowDefinition = workflowDefinition;
    }

    public virtual async Task<ExecutionResult> ExecuteAsync(StepExecutionContext context)
    {
        logger.Information("Execute step: {0}", Name);

        SetupProperties(context);

        return await RunAsync(context);
    }

    public virtual WorkflowStep? GetNextStep()
    {
        return NextStepId != null ? WorkflowDefinition.Steps[NextStepId] : null;
    }

    public void SetOutputVariable(string key, object value, Workflow workflow)
    {
        var variable = Outputs[key];
        variable.Value = value;

        workflow.SetVariable(variable);
    }

    public T GetInputValue<T>(string name)
    {
        if (Inputs[name].Value is JArray inputArray)
        {
            return inputArray.ToObject<T>();
        }

        return Inputs[name].GetValue<T>();
    }

    protected virtual void SetupProperties(StepExecutionContext context)
    {
        foreach (var input in Inputs)
        {
            var stepType = GetType();
            var stepVariable = stepType.GetProperty(input.Key);
            var variable = input.Value;
            var value = variable.Value;

            if (ShouldParseProperty(stepVariable))
            {
                value = StepVariableParser.Parse(variable, context.Workflow);
            }

            if (value == null)
            {
                continue;
            }

            if (value is JArray array)
            {
                value = array.ToObject(stepVariable.PropertyType);
            }

            if (stepVariable.PropertyType.Name == nameof(Guid))
            {
                Guid.TryParse(value.ToString(), out var guidValue);
                value = guidValue;
            }

            if (stepVariable.PropertyType.Name == nameof(String))
            {
                value = value?.ToString();
            }

            stepVariable.SetValue(this, value);
        }
    }

    private static bool ShouldParseProperty(PropertyInfo stepProperty)
    {
        var propertyParsing = stepProperty.GetCustomAttributes<IgnorePropertyParsing>().SingleOrDefault();

        return propertyParsing == null || !propertyParsing.Ignore;
    }
}
