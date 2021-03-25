using Automaton.Studio.Activities;
using Elsa.Activities.Console;
using Elsa.Client.Models;
using ElsaDashboard.Shared.Rpc;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows
    {
        private ICollection<WorkflowDefinition> Definitions { get; set; } = new List<WorkflowDefinition>();

        [Inject] private IWorkflowDefinitionService WorkflowDefinitionService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
        }

        protected override async Task OnParametersSetAsync()
        {
            await LoadWorkflowDefinitionsAsync();
        }

        private async Task LoadWorkflowDefinitionsAsync()
        {
            Definitions = (await WorkflowDefinitionService.ListAsync()).Items;
            //DefinitionGroupings = Definitions.GroupBy(x => x.DefinitionId).ToList();
        }
    }
}
