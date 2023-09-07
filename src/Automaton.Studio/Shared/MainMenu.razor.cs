using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Shared
{
    public partial class MainMenu : ComponentBase
    {
        [Parameter] public bool Collapsed { get; set; }
    }
}
