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

            ShowMenu(NavigationManager.Uri);

            await base.OnInitializedAsync();
        }

        public void SetCollapsed(bool collapsed)
        {
            this.collapsed = collapsed;

            Menu.SetCollapsed(collapsed);

            InvokeAsync(StateHasChanged);
        }

        private ValueTask NavigationLocationChanged(LocationChangingContext context)
        {
            ShowMenu(context.TargetLocation);

            InvokeAsync(StateHasChanged);

            return ValueTask.CompletedTask;
        }

        private void ShowMenu(string uri)
        {
            if (IsFlowMenu(uri))
            {
                var page = GetPage(uri);
                var parameters = GetFlowDesignerParams(uri, page);
                ShowFlowMenu(parameters.Item1, parameters.Item2);
            }
            else
            {
                ShowMainMenu();
            }
        }

        private bool IsFlowMenu(string uri)
        {
            return uri.Contains("flow/designer") || uri.Contains("flow/activity") || uri.Contains("flow/schedule");
        }

        private string GetPage(string uri)
        {
            if (uri.Contains("flow/designer"))
                return "flow/designer";
            else if (uri.Contains("flow/activity"))
                return "flow/activity";
            else if (uri.Contains("flow/schedule"))
                return "flow/schedule";

            return string.Empty;
        }

        private void ShowFlowMenu(Guid flowId, string flowName)
        {
            Menu = NavMenuViewModel.GetFlowMenu(flowId, flowName, collapsed);
        }

        private void ShowMainMenu()
        {
            Menu = NavMenuViewModel.GetMainMenu(collapsed);
        }

        private Tuple<Guid, string> GetFlowDesignerParams(string uri, string page)
        {
            var urlElements = uri.Split(page);
            var flowIdAndNameString = urlElements.LastOrDefault();
            var idAndNameArray = flowIdAndNameString.Trim('/').Split("/");
            var id = idAndNameArray[0];
            var flowName = idAndNameArray[1];
            Guid.TryParse(id, out var flowId);

            return new Tuple<Guid, string>(flowId, flowName);
        }
    }
}
