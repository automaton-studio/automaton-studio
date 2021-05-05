using Automaton.Studio.Components.ActionBar;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        #region Constants

        private const string WorkflowParam = "workflow";
        private const string PageRoute = "designer";

        #endregion

        #region Properties

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;
        [Inject] private IMainLayoutViewModel MainLayoutViewModel { get; set; } = default!;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            MainLayoutViewModel.ActionBar = ActionBarFactory.GetActionBar(PageRoute);

            await base.OnInitializedAsync();

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            
            if (queryStrings.TryGetValue(WorkflowParam, out var workflow))
            {
                await DesignerViewModel.LoadWorkflow(workflow);
            }

        }
    }
}
