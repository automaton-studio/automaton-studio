using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Steps.Sequence;

public partial class SequenceEndDesigner : ComponentBase
{
    [Parameter]
    public Domain.StudioStep Step { get; set; }

    [Parameter] 
    public RenderFragment ChildContent { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}
