using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        [Inject] private IWorkflowsViewModel WorkflowsViewModel { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await WorkflowsViewModel.LoadWorkflows();
        }

        private async Task RunWorkflow(string workflowId, string connectionId)
        {
            await WorkflowsViewModel.RunWorkflow(workflowId, connectionId);
        }
    }
}
