using Automaton.Core.Models;
using Automaton.WebApi.Interfaces;
using Automaton.WebApi.Models;

namespace Automaton.WebApi.Services
{
    public class FlowLoader : IFlowLoader
    {
        private readonly IServiceProvider serviceProvider;

        public FlowLoader(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Workflow LoadFlow(string source, Func<string, Flow> deserializer)
        {
            var sourceObj = deserializer(source);
            var def = LoadFlow(sourceObj);

            return def;
        }

        public Workflow LoadFlow(Flow flow)
        {
            var worklow = new Workflow
            {
                Id = flow.Id,
                Name = flow.Name,
                StartupDefinitionId = flow.StartupDefinitionId,
            };

            foreach (var definition in flow.Definitions)
            {
                var workflowDefinition = new WorkflowDefinition
                {
                    Id = definition.Id,
                    Steps = ConvertSteps(definition.Steps, worklow),
                    DefaultErrorBehavior = definition.DefaultErrorBehavior,
                    DefaultErrorRetryInterval = definition.DefaultErrorRetryInterval,
                    Description = definition.Description
                };

                worklow.Definitions.Add(workflowDefinition);
            }

            return worklow;
        }

        private List<WorkflowStep> ConvertSteps(ICollection<Step> steps, Workflow workflow)
        {
            var workflowSteps = new List<WorkflowStep>();

            foreach (var step in steps)
            {
                var stepType = FindType(step.Name);
                var workflowStep = serviceProvider.GetService(stepType) as WorkflowStep;

                workflowStep.Id = step.Id;
                workflowStep.Name = step.Name;
                workflowStep.ErrorBehavior = step.ErrorBehavior;
                workflowStep.RetryInterval = step.RetryInterval;

                AttachInputs(step, stepType, workflowStep);

                workflowSteps.Add(workflowStep);
            }

            return workflowSteps;
        }

        private void AttachInputs(Step step, Type stepType, WorkflowStep workflowStep)
        {
            throw new NotImplementedException();
        }

        private static Type FindType(string name)
        {
            return Type.GetType($"Automaton.Steps.{name}, Automaton.Steps", true, true);
        }
    }
}
