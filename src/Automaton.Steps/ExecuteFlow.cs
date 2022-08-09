using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Steps.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Steps;

public class ExecuteFlow : WorkflowStep
{
    private const string ApiConfigurationName = "ApiConfiguration";

    private readonly IConfiguration configuration;
    private readonly ApiConfig apiConfig;
    private readonly HttpClient httpClient;
    private WorkflowExecuteService workflowExecuteService;

    public Guid FlowId { get; set; }

    public ExecuteFlow
    (
        IConfiguration configuration, 
        HttpClient httpClient, 
        WorkflowExecuteService workflowExecuteService
    )
    {
        apiConfig = new ApiConfig();
        this.configuration = configuration;
        this.httpClient = httpClient;
        this.workflowExecuteService = workflowExecuteService;

        LoadApiConfig();
    }

    protected override Task<ExecutionResult> RunAsync(StepExecutionContext context)
    {
        Task.Run(async () => await RunFlow(FlowId));

        return Task.FromResult(ExecutionResult.Next());
    }

    public async Task RunFlow(Guid flowId)
    {
        var flow = await Load(flowId);
        await workflowExecuteService.Execute(flow);
    }

    private async Task<Flow> Load(Guid id)
    {
        var response = await httpClient.GetAsync($"{apiConfig.FlowsUrl}/{id}");
        var flow = await response.Content.ReadAsAsync<Flow>();

        return flow;
    }

    private void LoadApiConfig()
    {
        configuration.GetSection(ApiConfigurationName).Bind(apiConfig);
    }
}
