using Automaton.Studio.Domain;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Steps;

public partial class StepProperties : ComponentBase
{
    [Parameter]
    public StudioStep Step { get; set; }

    [Parameter] 
    public RenderFragment ChildContent { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}
