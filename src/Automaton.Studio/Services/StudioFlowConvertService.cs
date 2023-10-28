using Automaton.Core.Extensions;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Extensions;

namespace Automaton.Studio.Services;

public class StudioFlowConvertService
{
    private readonly IServiceProvider serviceProvider;

    public StudioFlowConvertService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public Workflow ConvertFlow(StudioFlow flow)
    {
        var workflow = new Workflow
        {
            Id = flow.Id,
            Name = flow.Name,
            Variables = flow.Variables,
            OutputVariables = flow.OutputVariables,
            StartupDefinitionId = flow.StartupDefinitionId
        };

        foreach (var definition in flow.Definitions)
        {
            var workflowDefinition = new WorkflowDefinition
            {
                Id = definition.Id
            };

            workflowDefinition.Steps = ConvertSteps(definition.Steps, workflowDefinition);

            workflow.Definitions.Add(workflowDefinition);
        }

        return workflow;
    }

    private IDictionary<string, WorkflowStep> ConvertSteps(ICollection<StudioStep> steps, WorkflowDefinition workflowDefinition)
    {
        var workflowSteps = new Dictionary<string, WorkflowStep>();

        foreach (var step in steps)
        {
            var workflowStep = serviceProvider.GetService(step.FindType()) as WorkflowStep;
            UpdateStep(ref workflowStep, step);
            workflowSteps.Add(step.Id, workflowStep);
        }

        return workflowSteps;
    }

    public void UpdateStep(ref WorkflowStep step, StudioStep studioStep)
    {
        step.Id = studioStep.Id;
        step.Name = studioStep.Name;
        step.DisplayName = studioStep.DisplayName;
        step.Description = studioStep.Description;
        step.Type = studioStep.Type;
        step.NextStepId = studioStep.NextStepId;
        step.ParentId = studioStep.ParentId;
        step.Inputs = studioStep.Inputs;
        step.Outputs = studioStep.Outputs;
    }
}
