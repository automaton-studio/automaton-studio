using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;

namespace Automaton.Studio.Pages.FlowDesigner;

public class FlowLogsViewModel
{
    private readonly FlowLogsService flowLogsService;
    private readonly IMapper mapper;

    public IEnumerable<LogModel> Logs { get; set; } = new List<LogModel>();
    public int Total { get; set; }

    public FlowLogsViewModel
    (
        FlowLogsService flowLogsService,
        IMapper mapper
    )
    {
        this.flowLogsService = flowLogsService;
        this.mapper = mapper;
    }

    public async Task GetLogs(string flowIdString, int startIndex, int pageSize)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var result = await flowLogsService.GetLogsResult(flowId, startIndex, pageSize);

        Logs = result.Logs;
        Total = result.Total;
    }

    public async Task<IEnumerable<LogModel>> GetLogs(string flowIdString, Guid flowExecutionId)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var logs = await flowLogsService.GetLogs(flowId, flowExecutionId);

        return logs;
    }

    public async Task<string> GetLogsText(string flowIdString, Guid flowExecutionId)
    {
        Guid.TryParse(flowIdString, out var flowId);
        var logsText = await flowLogsService.GetLogsActivityText(flowId, flowExecutionId);

        return logsText;
    }
}
