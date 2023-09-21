using AutoMapper;
using Automaton.Core.Enums;
using Automaton.Studio.Models;
using Automaton.Studio.Pages.Flows;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Runners;

public class FlowActivityViewModel
{
    private readonly FlowExecutionsService flowExecutionService;
    private readonly IMapper mapper;

    public ICollection<FlowExecution> Executions { get; set; } = new List<FlowExecution>();

    public FlowActivityViewModel
    (
        FlowExecutionsService flowExecutionService,
        FlowService flowService,
        IMapper mapper
    )
    {
        this.flowExecutionService = flowExecutionService;
        this.mapper = mapper;
    }

    public async Task GetFlowActivity(string flowIdString)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var activities = await flowExecutionService.ListForFlow(flowId);
        Executions = mapper.Map<ICollection<FlowExecution>>(activities);
    }

    public async Task<IEnumerable<LogModel>> GetLogs(string flowIdString, Guid flowExecutionId)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var logs = await flowExecutionService.GetLogsActivity(flowId, flowExecutionId);

        return logs;
    }

    public async Task<string> GetLogsctivity(string flowIdString, Guid flowExecutionId)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var logsText = await flowExecutionService.GetLogsActivityText(flowId, flowExecutionId);

        return logsText;
    }
}
