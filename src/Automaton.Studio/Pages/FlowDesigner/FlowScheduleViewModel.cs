using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;

namespace Automaton.Studio.Pages.Runners;

public class FlowScheduleViewModel
{
    private readonly FlowScheduleService flowScheduleService;
    private readonly IMapper mapper;

    public Guid FlowId { get; set; }
    public IList<FlowScheduleModel> Schedules { get; set; } = new List<FlowScheduleModel>();

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

    public async Task GetFlowSchedules(int startIndex, int pageSize)
    {
        Schedules = await flowScheduleService.GetFlowSchedules(FlowId);
    }

    public void AddNewSchedule()
    {
        var schedule = new FlowScheduleModel
        {
            Id = Guid.NewGuid(),
            Name = GetNewScheduleName(),
            FlowId = FlowId
        };

        Schedules.Insert(0, schedule);
    }

    private string GetNewScheduleName()
    {
        return $"Schedule {Schedules.Count}";
    }
}
