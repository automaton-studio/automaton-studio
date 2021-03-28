using Elsa.Client.Models;
using Elsa.Persistence;
using Elsa.Services;
using ElsaDashboard.Shared.Rpc;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        [Inject] private IWorkflowDefinitionService WorkflowDefinitionService { get; set; } = default!;
        [Inject] private IWorkflowRunner WorkflowRunner { get; set; } = default!;
        [Inject] private IWorkflowBlueprintMaterializer WorkflowBlueprintMaterializer { get; set; } = default!;
        [Inject] private IServiceProvider ServiceProvider { get; set; } = default!;
        [Inject] private IWorkflowDefinitionStore WorkflowDefinitionStore { get; set; } = default!;


        private ICollection<WorkflowDefinition> Definitions { get; set; } = new List<WorkflowDefinition>();

        protected override async Task OnInitializedAsync()
        {
            await LoadWorkflowDefinitionsAsync();
        }

        private async Task LoadWorkflowDefinitionsAsync()
        {
            Definitions = (await WorkflowDefinitionService.ListAsync()).Items;
        }

        private async Task RunWorkflow(string workflowId)
        {
            // Retrieve workflow definition from store.
            var storeWorkflowDefinition = await WorkflowDefinitionStore.FindAsync(new Elsa.Persistence.Specifications.WorkflowDefinitions.WorkflowDefinitionIdSpecification(workflowId));

            // Create blueprint from wokflow definition.
            var workflowBlueprint = await WorkflowBlueprintMaterializer.CreateWorkflowBlueprintAsync(storeWorkflowDefinition);

            // Execute workflow blueprint.
            await WorkflowRunner.RunWorkflowAsync(workflowBlueprint);
        }
    }
}
