using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Steps.If;

public partial class IfDesigner : ComponentBase
{
    [Parameter]
    public IfStep Step { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}
