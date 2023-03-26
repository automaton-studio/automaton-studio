using AutoMapper;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Flows;

public class FlowsViewModel
{
    private readonly FlowsService flowsService;
    private readonly RunnerService runnerService;
    private readonly FlowService flowService;
    private readonly IMapper mapper;

    public ICollection<FlowModel> Flows { get; set;  } = new List<FlowModel>();
    public ICollection<RunnerModel> Runners { get; set; } = new List<RunnerModel>();

    public FlowsViewModel
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

    public async Task GetFlows()
    {
        var flowsInfo = await flowsService.List();
        Flows = mapper.Map<ICollection<FlowModel>>(flowsInfo);
    }

    public async Task GetRunners()
    {
        Runners = await runnerService.List();
    }

    public async Task CreateFlow(string name)
    {
        var flow = await flowService.Create(name);

        var flowModel = new FlowModel
        {
            Id = flow.Id,
            Name = flow.Name,
            Created = flow.Created,
            Updated = flow.Updated
        };

        Flows.Add(flowModel);
    }

    public async Task DeleteFlow(Guid id)
    {
        await flowService.Delete(id);

        var flow = Flows.SingleOrDefault(x => x.Id == id);

        Flows.Remove(flow);
    }

    public async Task RunFlow(Guid id, IEnumerable<Guid> runnerIds)
    {
        await flowService.Run(id, runnerIds);
    }
}
