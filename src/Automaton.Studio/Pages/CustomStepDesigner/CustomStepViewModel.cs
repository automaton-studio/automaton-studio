using Automaton.Studio.Domain;
using Automaton.Studio.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomStepDesigner;

public class CustomStepViewModel
{
    private readonly CustomStepsService customStepsService;

    public CustomStep CustomStep { get; set; } = new();

    public CustomStepViewModel
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
