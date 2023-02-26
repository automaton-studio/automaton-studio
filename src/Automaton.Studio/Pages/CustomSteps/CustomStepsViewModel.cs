using AutoMapper;
using Automaton.Studio.Pages.CustomSteps.Components.NewCustomStep;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomSteps;

public class CustomStepsViewModel
{
    private readonly CustomStepsService customStepsService;
    private readonly IMapper mapper;

    public ICollection<CustomStepModel> CustomSteps { get; set;  } = new List<CustomStepModel>();

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
        CustomSteps = mapper.Map<ICollection<CustomStepModel>>(flowsInfo);
    }

    public async Task CreateCustomStep(NewCustomStepModel model)
    {
        var customStep = await customStepsService.Create(model.Name, model.DisplayName, model.Description);
        var customStepModel = mapper.Map<CustomStepModel>(customStep);

        CustomSteps.Add(customStepModel);
    }

    public async Task DeleteCustomStep(Guid id)
    {
        await customStepsService.Delete(id);

        var flow = CustomSteps.SingleOrDefault(x => x.Id == id);

        CustomSteps.Remove(flow);
    }
}
