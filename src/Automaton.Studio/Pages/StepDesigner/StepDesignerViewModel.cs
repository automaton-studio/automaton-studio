using AutoMapper;
using Automaton.Studio.Services;

namespace Automaton.Studio.Pages.StepDesigner;

public class StepDesignerViewModel
{
    private readonly FlowsService flowsService;
    private readonly RunnerService runnerService;
    private readonly FlowService flowService;
    private readonly IMapper mapper;

    public StepDesignerViewModel
    (
        FlowsService flowsService,
        RunnerService runnerService,
        FlowService flowService,
        IMapper mapper
    )
    {
        this.flowsService = flowsService;
        this.runnerService = runnerService;
        this.flowService = flowService;
        this.mapper = mapper;
    }
}
