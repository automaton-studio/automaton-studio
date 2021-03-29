using Elsa.Persistence;
using Elsa.Services;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Core
{
    public class WorkflowManager : IWorkflowManager
    {
        private IServiceProvider serviceProvider { get; set; }
        private IWorkflowRunner workflowRunner { get; set; }
        private IWorkflowDefinitionStore workflowDefinitionStore { get; set; }
        private IWorkflowBlueprintMaterializer workflowBlueprintMaterializer { get; set; }

        public WorkflowManager(IWorkflowRunner workflowRunner,
            IWorkflowBlueprintMaterializer workflowBlueprintMaterializer,
            IServiceProvider serviceProvider,
            IWorkflowDefinitionStore workflowDefinitionStore)
        {
            this.serviceProvider = serviceProvider;
            this.workflowRunner = workflowRunner;
            this.workflowDefinitionStore = workflowDefinitionStore;
            this.workflowBlueprintMaterializer = workflowBlueprintMaterializer;
        }

        public async Task RunWorkflow(string workflowId)
        {
            // Retrieve workflow definition from store.
            var storeWorkflowDefinition = await workflowDefinitionStore.FindAsync(new Elsa.Persistence.Specifications.WorkflowDefinitions.WorkflowDefinitionIdSpecification(workflowId));

            // Create blueprint from wokflow definition.
            var workflowBlueprint = await workflowBlueprintMaterializer.CreateWorkflowBlueprintAsync(storeWorkflowDefinition);

            // Execute workflow blueprint.
            await workflowRunner.RunWorkflowAsync(workflowBlueprint);
        }
    }
}
