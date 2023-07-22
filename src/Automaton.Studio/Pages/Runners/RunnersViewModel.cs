using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Runners;

public class RunnersViewModel
{
    private readonly RunnerService runnerService;
    private readonly IMapper mapper;

    public ICollection<RunnerModel> Runners { get; set; } = new List<RunnerModel>();

    public RunnersViewModel
    (
        RunnerService runnerService,
        FlowService flowService,
        IMapper mapper
    )
    {
        this.runnerService = runnerService;
        this.mapper = mapper;
    }

    public async Task GetRunners()
    {
        var runners = await runnerService.List();
        Runners = mapper.Map<ICollection<RunnerModel>>(runners);
    }
}
