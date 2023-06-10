using Automaton.Studio.Steps.Test;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Steps.Sequence;

public partial class TestDesigner : ComponentBase
{
    [Parameter]
    public TestStep Step { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}
