using Automaton.Core.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class FlowService
{
    private readonly HttpClient httpClient;
    private readonly ConfigurationService configService;
    private readonly RunnerFlowExecuteService flowExecuteService;

    public FlowService(RunnerFlowExecuteService workflowExecuteService, 
        ConfigurationService configService, 
        HttpClient httpClient)
    {
        this.flowExecuteService = workflowExecuteService;
        this.configService = configService;
        this.httpClient = httpClient;
    }

    public async Task ExecuteFlow(Guid flowId)
    {
        var flow = await Load(flowId);

        await flowExecuteService.Execute(flow);
    }

    private async Task<Flow> Load(Guid id)
    {
        var response = await httpClient.GetAsync($"{configService.FlowsUrl}/{id}");
        var flow = await response.Content.ReadAsAsync<Flow>();

        return flow;
    }
}
