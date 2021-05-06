using Automaton.Studio.Models;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IWorkflowsViewModel WorkflowsViewModel { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await WorkflowsViewModel.Initialize();
        }

        private async Task RunWorkflow(WorkflowModel workflow)
        {
            await WorkflowsViewModel.RunWorkflow(workflow);
        }

        private void EditWorkflow(WorkflowModel workflow)
        {
            NavigationManager.NavigateTo($"designer/{workflow.DefinitionId}");
        }
    }
}
