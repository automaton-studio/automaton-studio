using Automaton.Studio.Components.ActionBar;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages
{
    partial class Index : ComponentBase
    {
        [Inject] private IMainLayoutViewModel MainLayoutViewModel { get; set; } = default!;

        protected override void OnInitialized()
        {
            MainLayoutViewModel.ActionBar = ActionBarFactory.GetActionBar(string.Empty);
        }
    }
}
