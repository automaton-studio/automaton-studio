using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;

namespace Automaton.Studio.Pages.Runners;

public class FlowScheduleViewModel
{
    private readonly FlowScheduleService flowScheduleService;
    private readonly IMapper mapper;

    public IEnumerable<FlowSchedule> Schedules { get; set; } = new List<FlowSchedule>();

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

    public async Task GetFlowSchedules(string flowIdString, int startIndex, int pageSize)
    {
        Guid.TryParse(flowIdString, out var flowId);
        Schedules = await flowScheduleService.GetFlowSchedules(flowId);
    }
}
