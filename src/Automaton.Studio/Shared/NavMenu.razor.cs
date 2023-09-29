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
            if (context.TargetLocation.Contains("flowdesigner/"))
            {
                var urlElements = context.TargetLocation.Split("flowdesigner/");
                var flowIdAndNameString = urlElements.LastOrDefault();
                var idAndNameArray = flowIdAndNameString.Split("/");
                var id = idAndNameArray[0];
                var flowName = idAndNameArray[1];
                Guid.TryParse(id, out var flowId);
                ShowFlowMenu(flowId, flowName);
            }
            else if (context.TargetLocation.Equals(NavigationManager.BaseUri, StringComparison.OrdinalIgnoreCase))
            {
                ShowMainMenu();
            }

            InvokeAsync(StateHasChanged);

            return ValueTask.CompletedTask;
        }

        public void ShowFlowMenu(Guid flowId, string flowName)
        {
            Menu = NavMenuViewModel.GetFlowMenu(flowId, flowName, collapsed);
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
