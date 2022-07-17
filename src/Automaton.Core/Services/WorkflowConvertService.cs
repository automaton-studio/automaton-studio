using Automaton.Core.Models;
using Automaton.Studio.Extensions;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Automaton.Core.Services;

public class WorkflowConvertService
{
    private readonly IServiceProvider serviceProvider;

    public WorkflowConvertService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public Workflow ConvertFlow(Flow flow)
    {
        var worklow = new Workflow
        {
            Id = flow.Id,
            Name = flow.Name,
            StartupDefinitionId = flow.StartupDefinitionId,
            Variables = flow.Variables
        };

        foreach (var definition in flow.Definitions)
        {
            var workflowDefinition = new WorkflowDefinition
            {
                Id = definition.Id,
                Steps = ConvertSteps(definition.Steps, worklow),
                DefaultErrorBehavior = definition.DefaultErrorBehavior,
                DefaultErrorRetryInterval = definition.DefaultErrorRetryInterval
            };

            worklow.Definitions.Add(workflowDefinition);
        }

        return worklow;
    }

    private IDictionary<string, WorkflowStep> ConvertSteps(ICollection<Step> steps, Workflow workflow)
    {
        var workflowSteps = new Dictionary<string, WorkflowStep>();

        foreach (var step in steps)
        {
            var workflowStep = serviceProvider.GetService(step.FindType()) as WorkflowStep;
            workflowStep.Id = step.Id;
            workflowStep.Name = step.Name;
            workflowStep.Type = step.Type;
            workflowStep.NextStepId = step.NextStepId;
            workflowStep.ErrorBehavior = step.ErrorBehavior;
            workflowStep.RetryInterval = step.RetryInterval;

            AttachInputs(step, workflowStep, workflow);
            AttachOutputs(step, workflowStep, workflow);

            workflowSteps.Add(step.Id, workflowStep);
        }

        return workflowSteps;
    }

    private static void AttachInputs(Step step, WorkflowStep workflowStep, Workflow workflow)
    {
        foreach (var input in step.Inputs)
        {
            var stepType = step.FindType();

            var inputProperty = stepType.GetProperty(input.Key);

            var value = step.Inputs[input.Key];

            if (value is JArray array)
            {
                value = array.ToObject(inputProperty.PropertyType);
            }

            var inputValue = ParseValue(value, workflow);

            inputProperty.SetValue(workflowStep, inputValue);
        }

        workflowStep.Inputs = step.Inputs;
    }

    private static void AttachOutputs(Step step, WorkflowStep workflowStep, Workflow workflow)
    {
        foreach (var output in step.Outputs)
        {
            var stepType = step.FindType();

            var outputProperty = stepType.GetProperty(output.Key);

            var value = step.Outputs[output.Key];

            if (value is JArray array)
            {
                value = array.ToObject(outputProperty.PropertyType);
            }

            var outputValue = ParseValue(value, workflow);

            outputProperty.SetValue(workflowStep, outputValue);
        }

        workflowStep.Outputs = step.Outputs;
    }

    private static object ParseValue(object inputValue, Workflow workflow)
    {
        return ValueHasVariables(inputValue) ? ParseValueWithVariables(inputValue, workflow) : inputValue;
    }

    private static object ParseValueWithVariables(object inputValue, Workflow workflow)
    {
        var variableNames = GetVariableNames(inputValue);
        var parameterExpressions = GetVariableExpressions(variableNames, workflow);

        var expresion = inputValue.ToString().Replace("%", string.Empty);
        var lambdaExpresion = DynamicExpressionParser.ParseLambda(parameterExpressions.ToArray(), null, expresion);

        var workflowVariables = workflow.GetVariables(variableNames);
        var variableValues = workflowVariables.Select(x => x.Value);

        var value = lambdaExpresion.Compile().DynamicInvoke(variableValues.ToArray());

        return value;
    }

    private static bool ValueHasVariables(object inputValue)
    {
        var inputString = inputValue.ToString();

        var result = inputString.Split().Any(x => x.StartsWith("%") && x.EndsWith("%"));

        return result;
    }

    private static IEnumerable<string> GetVariableNames(object inputValue)
    {
        var inputString = inputValue.ToString();

        var variableNames = inputString.Split()
            .Where(x => x.StartsWith("%") && x.EndsWith("%"))
            .Select(x => x.Replace("%", string.Empty));

        return variableNames;
    }

    private static IEnumerable<ParameterExpression> GetVariableExpressions(IEnumerable<string> variableNames, Workflow workflow)
    {
        var variableExpressions = new List<ParameterExpression>();

        foreach (var name in variableNames)
        {
            var variable = workflow.GetVariable(name);
            var variableExpression = Expression.Parameter(variable.Value.GetType(), variable.Key);
            variableExpressions.Add(variableExpression);
        }

        return variableExpressions;
    }
}
