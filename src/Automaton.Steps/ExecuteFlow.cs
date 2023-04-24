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
    public IList<StepVariable> InputVariables { get; set; }
    public IList<StepVariable> OutputVariables { get; set; }

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
        Task.Run(async () =>
        {
            var flow = await Load(FlowId);

            var inputVariables = InputVariables.ToDictionary(x => x.Name, x => (object)x.Value);

            SetFlowInputVariables(flow, inputVariables);

            await workflowExecuteService.Execute(flow);

            SetStepOutputVariables(flow.OutputVariables, context);
        });

        return Task.FromResult(ExecutionResult.Next());
    }

    private async Task<Flow> Load(Guid id)
    {
        var response = await httpClient.GetAsync($"{configService.ApiConfig.FlowsUrl}/{id}");
        var flow = await response.Content.ReadAsAsync<Flow>();

        return flow;
    }

    private static void SetFlowInputVariables(Flow flow, IDictionary<string?, object?> inputVariables)
    {
        foreach (var key in inputVariables.Keys)
        {
            if (flow.InputVariables.ContainsKey(key))
            {
                flow.InputVariables[key] = inputVariables[key];
            }
        }
    }

    private void SetStepOutputVariables(IDictionary<string?, object?> outputVariables, StepExecutionContext context)
    {
        foreach (var variable in OutputVariables)
        {
            if (outputVariables.ContainsKey(variable.Name))
            {
                variable.Value = outputVariables[variable.Name].ToString();
                context.Workflow.Variables[variable.Name] = variable;
            }
        }
    }
}
