using AutoMapper;
using Automaton.Studio.Models;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomSteps;

public class CustomStepsViewModel
{
    private readonly CustomStepsService customStepsService;
    private readonly IMapper mapper;

    public ICollection<CustomStepListItem> CustomSteps { get; set;  } = new List<CustomStepListItem>();

    public CustomStepsViewModel
    (
        CustomStepsService customStepsService,
        IMapper mapper
    )
    {
        this.customStepsService = customStepsService;
        this.mapper = mapper;
    }

    public async Task GetCustomSteps()
    {
        var flowsInfo = await customStepsService.List();
        CustomSteps = mapper.Map<ICollection<CustomStepListItem>>(flowsInfo);
    }

    public async Task CreateCustomStep(NewCustomStep customStep)
    {
        var step = await customStepsService.Create(customStep);
        var customStepModel = mapper.Map<CustomStepListItem>(step);

        CustomSteps.Add(customStepModel);
    }

    public async Task DeleteCustomStep(Guid id)
    {
        await customStepsService.Delete(id);

        var flow = CustomSteps.SingleOrDefault(x => x.Id == id);

        CustomSteps.Remove(flow);
    }
}
