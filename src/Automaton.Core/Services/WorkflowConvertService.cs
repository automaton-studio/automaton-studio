using Automaton.Core.Models;
using Automaton.Studio.Extensions;
using Newtonsoft.Json.Linq;

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

            inputProperty.SetValue(workflowStep, value);
        }

        workflowStep.Inputs = step.Inputs;
    }
}
