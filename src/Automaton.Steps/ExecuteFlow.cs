using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Steps.Config;

namespace Automaton.Steps;

public class ExecuteFlow : WorkflowStep
{
    private readonly ConfigService configService;
    private readonly HttpClient httpClient;
    private WorkflowExecuteService workflowExecuteService;

    public Guid FlowId { get; set; }

    public ExecuteFlow
    (
        ConfigService configService, 
        HttpClient httpClient, 
        WorkflowExecuteService workflowExecuteService
    )
    {
        this.configService = configService;
        this.httpClient = httpClient;
        this.workflowExecuteService = workflowExecuteService;
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
        var response = await httpClient.GetAsync($"{configService.ApiConfig.FlowsUrl}/{id}");
        var flow = await response.Content.ReadAsAsync<Flow>();

        return flow;
    }
}
