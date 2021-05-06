using Automaton.Studio.Components.ActionBar;
using Automaton.Studio.Enums;
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

        #endregion

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;
        [Inject] private IMainLayoutViewModel MainLayoutViewModel { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Update MainLayout ActionBar
            MainLayoutViewModel.ActionBar = ActionBarFactory.GetActionBar(ActionBar.Designer);

            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);
            var queryStrings = QueryHelpers.ParseQuery(uri.Query);
            
            if (queryStrings.TryGetValue(WorkflowParam, out var workflow))
            {
                await DesignerViewModel.LoadWorkflow(workflow);
            }
        }
    }
}
