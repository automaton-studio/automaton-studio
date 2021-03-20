using ElsaDashboard.Shared.Rpc;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class WorkflowDefinitions
    {
        private int currentCount = 0;

        [Inject] private IWorkflowDefinitionService WorkflowDefinitionService { get; set; } = default!;
        [Inject] private IActivityService ActivityService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            var workflowDefinitions = await WorkflowDefinitionService.ListAsync();
        }


        private void IncrementCount()
        {
            currentCount++;
        }
    }
}
