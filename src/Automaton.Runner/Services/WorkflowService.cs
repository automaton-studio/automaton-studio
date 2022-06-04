using Automaton.Client.Auth.Http;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Runner.Core.Services;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Runner.Services;

public class WorkflowService
{
    private readonly HttpClient httpClient;
    private readonly ConfigService configService;
    private readonly WorkflowExecuteService workflowExecuteService;

    public WorkflowService(WorkflowExecuteService workflowExecuteService, 
        ConfigService configService, 
        AutomatonHttpClient httpClient)
    {
        this.workflowExecuteService = workflowExecuteService;
        this.configService = configService;
        this.httpClient = httpClient.Client;
    }

    public async Task RunWorkflow(Guid workflowId)
    {
        //await workflowExecuteService.Execute(workflowId);
    }

    private async Task<Flow> Load(Guid id)
    {
        var response = await httpClient.GetAsync($"{configService.ApiConfig.FlowsUrl}/{id}");
        var flow = await response.Content.ReadAsAsync<Flow>();

        return flow;
    }
}
