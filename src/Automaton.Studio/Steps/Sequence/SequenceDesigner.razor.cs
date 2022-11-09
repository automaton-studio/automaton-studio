using AntDesign;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Automaton.Studio.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps.Sequence;

public partial class SequenceDesigner : ComponentBase
{
    [Parameter]
    public StudioStep Step { get; set; }

    [Parameter] 
    public RenderFragment ChildContent { get; set; }

    [Inject] 
    private ModalService ModalService { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private async Task OnEdit(StudioStep step)
    {
        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () => {

            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private static void OnDelete(StudioStep step)
    {
        step.Definition.DeleteStep(step);
    }

    private void Collapse()
    {
        var children = (Step as SequenceStep).GetChildren();

        foreach (var child in children)
        {
            child.Hidden = true;
        }
    }

    private void Expand()
    {
        var children = (Step as SequenceStep).GetChildren();

        foreach (var child in children)
        {
            child.Hidden = false;
        }
    }

}
