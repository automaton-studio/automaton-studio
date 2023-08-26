using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Runner.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class FlowService
{
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly WorkflowExecuteService workflowExecuteService;

    public FlowService(WorkflowExecuteService workflowExecuteService, 
        ConfigurationService configService, 
        RunnerHttpClient httpClient)
    {
        this.workflowExecuteService = workflowExecuteService;
        this.configService = configService;
        this.httpClient = httpClient.Client;
    }

    public async Task ExecuteWorkflow(Guid flowId)
    {
        var flow = await Load(flowId);

        await workflowExecuteService.Execute(flow);
    }

    private async Task<Flow> Load(Guid id)
    {
        var response = await httpClient.GetAsync($"{configService.FlowsUrl}/{id}");
        var flow = await response.Content.ReadAsAsync<Flow>();

        return flow;
    }
}
