using AntDesign;
using Automaton.Studio.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps;

public partial class StepDesigner : ComponentBase
{
    [Parameter]
    public Domain.StudioStep Step { get; set; }

    [Parameter] 
    public RenderFragment ChildContent { get; set; }

    [Inject] 
    private ModalService ModalService { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private async Task OnEdit(Domain.StudioStep step)
    {
        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () => {

            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private static void OnDelete(Domain.StudioStep step)
    {
        step.Definition.DeleteStep(step);
    }
}
