using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;

namespace Automaton.Studio.Services
{
    public class FlowConvertService : IFlowConvertService
    {
        private readonly ConfigService configService;
        private readonly IServiceProvider serviceProvider;

        public FlowConvertService
        (
            IServiceProvider serviceProvider, 
            ConfigService configService
        )
        {
            this.serviceProvider = serviceProvider;
            this.configService = configService;
        }

        public Workflow ConvertFlow(Flow flow)
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
                    Steps = ConvertSteps(definition.Steps),
                    DefaultErrorBehavior = definition.DefaultErrorBehavior,
                    DefaultErrorRetryInterval = definition.DefaultErrorRetryInterval,
                    Description = definition.Description
                };

                worklow.Definitions.Add(workflowDefinition);
            }

            return worklow;
        }

        private IDictionary<string, WorkflowStep> ConvertSteps(ICollection<Step> steps)
        {
            var workflowSteps = new Dictionary<string, WorkflowStep>();

            foreach (var step in steps)
            {
                var stepType = FindType(step.Type);

                var workflowStep = serviceProvider.GetService(stepType) as WorkflowStep;
                workflowStep.Id = step.Id;
                workflowStep.Name = step.Name;
                workflowStep.Type = step.Type;
                workflowStep.NextStepId = step.NextStepId;
                workflowStep.ErrorBehavior = step.ErrorBehavior;
                workflowStep.RetryInterval = step.RetryInterval;

                AttachInputs(step, stepType, workflowStep);

                workflowSteps.Add(step.Id, workflowStep);
            }

            return workflowSteps;
        }

        private void AttachInputs(Step step, Type stepType, WorkflowStep workflowStep)
        {
            foreach (var input in step.Inputs)
            {
                var inputProperty = stepType.GetProperty(input.Key);

                if (inputProperty == null)
                {
                    throw new ArgumentException($"Unknown property for input {input.Key} on {step.Name}");
                }

                var expresion = Convert.ToString(input.Value);
                var lambdaExpresion = DynamicExpressionParser.ParseLambda(typeof(object), expresion);
                var value = lambdaExpresion.Compile().DynamicInvoke();

                inputProperty.SetValue(workflowStep, value);
            }
        }

        private static Type? FindType(string name)
        {
            var fullClassName = $"Automaton.Steps.{name}, Automaton.Steps";

            var type = Type.GetType(fullClassName, true, true);

            return type;
        }
    }
}
