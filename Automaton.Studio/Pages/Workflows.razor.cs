using Elsa.Client.Models;
using ElsaDashboard.Shared.Rpc;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        [Inject] private IWorkflowDefinitionService WorkflowDefinitionService { get; set; } = default!;

        private ICollection<WorkflowDefinition> Definitions { get; set; } = new List<WorkflowDefinition>();

        protected override async Task OnInitializedAsync()
        {
            await LoadWorkflowDefinitionsAsync();
        }

        private async Task LoadWorkflowDefinitionsAsync()
        {
            Definitions = (await WorkflowDefinitionService.ListAsync()).Items;
        }
    }
}
