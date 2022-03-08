using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Extensions;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Automaton.Studio.Services
{
    public class FlowConvertService : IFlowConvertService
    {
        private readonly ConfigService configService;
        private readonly IServiceProvider serviceProvider;
        private readonly IMapper mapper;

        public FlowConvertService
        (
            IServiceProvider serviceProvider, 
            ConfigService configService,
            IMapper mapper
        )
        {
            this.serviceProvider = serviceProvider;
            this.configService = configService;
            this.mapper = mapper;
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
                    DefaultErrorRetryInterval = definition.DefaultErrorRetryInterval,
                    Description = definition.Description
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

                if (inputProperty == null)
                {
                    throw new ArgumentException($"Unknown property for input {input.Key} on {step.Name}");
                }

                foreach (var variable in workflow.Variables)
                {
                    var variableExpression = Expression.Parameter(variable.Value.GetType(), variable.Key);
                }

                var expresion = Convert.ToString(input.Value);
                var lambdaExpresion = DynamicExpressionParser.ParseLambda(workflow.VariableExpressions.ToArray(), typeof(object), expresion);

                var value = lambdaExpresion.Compile().DynamicInvoke(workflow.GetVariableValues());

                inputProperty.SetValue(workflowStep, value);
            }
        }       
    }
}
