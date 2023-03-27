using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services;

public class FlowService
{
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly IMapper mapper;
    private readonly ILogger<FlowService> logger;
    private readonly StepFactory stepFactory;

    public FlowService
    (
        ConfigurationService configService,
        IMapper mapper,
        HttpClient httpClient,
        ILogger<FlowService> logger,
        StepFactory stepFactory
    )
    {
        this.logger = logger;
        this.configService = configService;
        this.httpClient = httpClient;
        this.mapper = mapper;
        this.stepFactory = stepFactory;
    }

    public async Task<StudioFlow> Load(Guid id)
    {
        var response = await httpClient.GetAsync($"{configService.FlowsUrl}/{id}");
        response.EnsureSuccessStatusCode();

        var flow = await response.Content.ReadAsAsync<Flow>();
        var studioFlow = CreateStudioFlow(flow);

        return studioFlow;
    }

    public async Task<StudioFlow> Create(string name)
    {
        var defaultDefinition = new Definition();

        var flow = new Flow 
        { 
            Name = name,
            StartupDefinitionId = defaultDefinition.Id,
            Definitions = new List<Definition> { defaultDefinition }
        };

        var response = await httpClient.PostAsJsonAsync(configService.FlowsUrl, flow);
        response.EnsureSuccessStatusCode();

        flow = await response.Content.ReadAsAsync<Flow>();

        var studioFlow = CreateStudioFlow(flow);

        return studioFlow;
    }

    public async Task Update(StudioFlow flow)
    {
        var flowDto = mapper.Map<Flow>(flow);

        var response = await httpClient.PutAsJsonAsync($"{configService.FlowsUrl}/{flow.Id}", flowDto);

        response.EnsureSuccessStatusCode();
    }

    public async Task Delete(Guid flowId)
    {
        var response = await httpClient.DeleteAsync($"{configService.FlowsUrl}/{flowId}");

        response.EnsureSuccessStatusCode();
    }
   
    public async Task Run(Guid flowId, IEnumerable<Guid> runnerIds)
    {
        var flowAndRunners = new
        {
            FlowId = flowId,
            RunnerIds = runnerIds
        };

        var response = await httpClient.PostAsJsonAsync($"{configService.FlowsUrl}/run", flowAndRunners);
        
        response.EnsureSuccessStatusCode();
    }

    private StudioFlow CreateStudioFlow(Flow flow)
    {
        var studioFlow = new StudioFlow
        {
            Id = flow.Id,
            Name = flow.Name,
            Created = flow.Created,
            Updated = flow.Updated,
            StartupDefinitionId = flow.StartupDefinitionId,
            InputVariables = flow.InputVariables,
            OutputVariables = flow.OutputVariables,
            Variables = flow.Variables,
            Definitions = new List<StudioDefinition>()
        };

        var studioDefinitions = CreateStudioDefinitions(flow.Definitions, studioFlow);

        studioFlow.Definitions.AddRange(studioDefinitions);

        return studioFlow;
    }

    private IEnumerable<StudioDefinition> CreateStudioDefinitions(IEnumerable<Definition> definitions, StudioFlow studioFlow)
    {
        foreach (var definition in definitions)
        {
            var studioDefinition = new StudioDefinition
            {
                Id = definition.Id,
                Name = definition.Name,
                Flow = studioFlow,                
                Steps = new List<StudioStep>(),
            };

            var studioSteps = CreateSteps(definition.Steps);

            studioDefinition.Steps.AddRange(studioSteps);

            yield return studioDefinition;
        }
    }

    private IEnumerable<StudioStep> CreateSteps(IEnumerable<Step> steps)
    {
        foreach (var step in steps)
        {
            var studioStep = stepFactory.CreateStep(step);

           

            yield return studioStep;
        }
    }
}
