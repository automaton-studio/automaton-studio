using Automaton.Studio.Components.ActionBar;
using Automaton.Studio.Models;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        #region Constants

        private const string PageRoute = "workflows";

        #endregion

        [Inject] private IWorkflowsViewModel WorkflowsViewModel { get; set; } = default!;
        [Inject] private IMainLayoutViewModel MainLayoutViewModel { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            MainLayoutViewModel.ActionBar = ActionBarFactory.GetActionBar(PageRoute);

            await WorkflowsViewModel.Initialize();
        }

        private async Task RunWorkflow(WorkflowModel workflow)
        {
            await WorkflowsViewModel.RunWorkflow(workflow);
        }
    }
}
