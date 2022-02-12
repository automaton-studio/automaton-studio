using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Errors;
using Automaton.Studio.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public class FlowService : IFlowService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly HttpClient httpClient;
        private readonly ConfigService configService;
        private readonly IMapper mapper;
        private readonly ILogger<FlowService> logger;

        public FlowService
        (
            IServiceProvider serviceProvider,
            ConfigService configService,
            IMapper mapper,
            HttpClient httpClient,
            ILogger<FlowService> logger
        )
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
            this.configService = configService;
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FlowModel>> List()
        {
            try
            {
                var response = await httpClient.GetAsync($"{configService.FlowsUrl}");

                var flows = await response.Content.ReadAsAsync<IEnumerable<FlowModel>>();

                return flows;
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, "Failed to load flows list");

                return new List<FlowModel>();
            }
        }

        public async Task<Flow> Load(string id)
        {
            var response = await httpClient.GetAsync($"{configService.FlowsUrl}/{id}");
            var conductorFlow = await response.Content.ReadAsAsync<Conductor.Flow>();
            var flow = mapper.Map<Flow>(conductorFlow);
            
            return flow;
        }

        public async Task Create(Flow flow)
        {
            var json = JsonSerializer.Serialize(flow);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            await httpClient.PostAsync(configService.FlowsUrl, requestContent);
        }

        public async Task Update(Flow flow)
        {
            var json = JsonSerializer.Serialize(flow);
            var requestContent = new StringContent(json, Encoding.UTF8, "application/json");

            await httpClient.PutAsync($"{configService.FlowsUrl}/{flow.Id}", requestContent);
        }

        public async Task Delete(string flowId)
        {
            await httpClient.DeleteAsync($"{configService.FlowsUrl}/{flowId}");
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
