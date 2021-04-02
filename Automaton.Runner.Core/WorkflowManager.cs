using Elsa.Persistence;
using Elsa.Services;
using System.Threading.Tasks;

namespace Automaton.Runner.Core
{
    public class WorkflowManager : IWorkflowManager
    {
        private readonly IWorkflowRunner workflowRunner;
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;
        private readonly IWorkflowBlueprintMaterializer workflowBlueprintMaterializer;

        public WorkflowManager(IWorkflowRunner workflowRunner,
            IWorkflowBlueprintMaterializer workflowBlueprintMaterializer,
            IWorkflowDefinitionStore workflowDefinitionStore)
        {
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
