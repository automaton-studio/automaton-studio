using Automaton.Studio.Hubs;
using Elsa.Client.Models;
using ElsaDashboard.Shared.Rpc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        [Inject] private IWorkflowDefinitionService WorkflowDefinitionService { get; set; } = default!;
        [Inject] private IHubContext<WorkflowHub> WorkflowHubContext { get; set; } = default!;     

        private ICollection<WorkflowDefinition> Definitions { get; set; } = new List<WorkflowDefinition>();

        protected override async Task OnInitializedAsync()
        {
            Definitions = (await WorkflowDefinitionService.ListAsync()).Items;
        }

        private async Task RunWorkflow(string workflowId)
        {
            await WorkflowHubContext.Clients.All.SendAsync("RunWorkflow", workflowId);
        }
    }
}
