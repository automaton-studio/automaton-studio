using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;

namespace Automaton.Studio.Pages.Runners;

public class FlowScheduleViewModel
{
    private readonly FlowScheduleService flowScheduleService;
    private readonly IMapper mapper;

    public IEnumerable<FlowExecution> Executions { get; set; } = new List<FlowExecution>();
    public int Total { get; set; }

    public FlowScheduleViewModel
    (
        FlowScheduleService flowScheduleService,
        FlowService flowService,
        IMapper mapper
    )
    {
        this.flowScheduleService = flowScheduleService;
        this.mapper = mapper;
    }

    public async Task GetFlowActivity(string flowIdString, int startIndex, int pageSize)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var result = await flowScheduleService.GetFlowExecutionResult(flowId, startIndex, pageSize);

        Executions = result.FlowExecutions;
        Total = result.Total;
    }

    public async Task<IEnumerable<LogModel>> GetLogs(string flowIdString, Guid flowExecutionId)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var logs = await flowScheduleService.GetLogsActivity(flowId, flowExecutionId);

        return logs;
    }

    public async Task<string> GetLogsText(string flowIdString, Guid flowExecutionId)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var logsText = await flowScheduleService.GetLogsActivityText(flowId, flowExecutionId);

        return logsText;
    }
}
