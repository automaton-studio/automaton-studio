using Automaton.Core.Models;
using Automaton.Studio.Extensions;

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

    private IDictionary<string, WorkflowStep> ConvertSteps(ICollection<Step> steps, WorkflowDefinition workflowDefinition)
    {
        var workflowSteps = new Dictionary<string, WorkflowStep>();

        foreach (var step in steps)
        {
            var workflowStep = serviceProvider.GetService(step.FindType()) as WorkflowStep;
            workflowStep.Setup(step, workflowDefinition);
            workflowSteps.Add(step.Id, workflowStep);
        }

        return workflowSteps;
    } 
}
