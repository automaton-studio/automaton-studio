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

    public async Task GetFlowActivity()
    {
        var activities = await flowExecutionService.List();
        Executions = mapper.Map<ICollection<FlowExecution>>(activities);
    }
}
