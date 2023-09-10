using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace Automaton.Studio.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool collapsed;

        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] NavMenuViewModel NavMenuViewModel { get; set; }

        public MenuMetadata Menu { get; set; }

        protected override async Task OnInitializedAsync()
        {
            NavigationManager.RegisterLocationChangingHandler(NavigationLocationChanged);

            Menu = NavMenuViewModel.GetMainMenu(collapsed);

            await base.OnInitializedAsync();
        }

        private ValueTask NavigationLocationChanged(LocationChangingContext context)
        {
            if (context.TargetLocation.Contains("flowdesigner"))
            {
                var id = context.TargetLocation.Split('/')[1];
                Guid.TryParse(id, out var flowId);
                ShowFlowMenu(flowId);
            }
            else if (context.TargetLocation.Equals(NavigationManager.BaseUri, StringComparison.OrdinalIgnoreCase))
            {
                ShowMainMenu();
            }

            InvokeAsync(StateHasChanged);

            return ValueTask.CompletedTask;
        }

        public void ShowFlowMenu(Guid flowId)
        {
            Menu = NavMenuViewModel.GetFlowMenu(flowId, collapsed);
        }

        public void ShowMainMenu()
        {
            Menu = NavMenuViewModel.GetMainMenu(collapsed);
        }

        public void SetCollapsed(bool collapsed)
        {
            this.collapsed = collapsed;

            Menu.SetCollapsed(collapsed);

            InvokeAsync(StateHasChanged);
        }
    }
}
