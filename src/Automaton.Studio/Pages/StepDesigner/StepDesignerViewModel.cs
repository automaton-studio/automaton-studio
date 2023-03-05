using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.StepDesigner;

public class StepDesignerViewModel
{
    private readonly CustomStepsService customStepsService;

    public CustomStep CustomStep { get; set; }

    public StepDesignerViewModel
    (
        CustomStepsService customStepsService
    )
    {
        this.customStepsService = customStepsService;
    }

    public async Task Load(Guid id)
    {
        CustomStep = await customStepsService.Load(id);
    }

    public async Task Save()
    {
        await customStepsService.Update(CustomStep);
    }
}
