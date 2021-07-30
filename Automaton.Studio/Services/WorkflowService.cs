using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Services;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;
        private readonly IWorkflowBlueprintMaterializer workflowBlueprintMaterializer;
        private readonly IStartsWorkflow startsWorkflow;

        public WorkflowService(
            IWorkflowBlueprintMaterializer workflowBlueprintMaterializer,
            IWorkflowDefinitionStore workflowDefinitionStore,
            IStartsWorkflow startsWorkflow)
        {
            this.workflowDefinitionStore = workflowDefinitionStore;
            this.workflowBlueprintMaterializer = workflowBlueprintMaterializer;
            this.startsWorkflow = startsWorkflow;
        }

        public async Task RunWorkflow(string workflowId)
        {
            // Retrieve workflow definition from store.
            var workflowDefinition = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(workflowId));

            // Create blueprint from wokflow definition.
            var workflowBlueprint = await workflowBlueprintMaterializer.CreateWorkflowBlueprintAsync(workflowDefinition);

            // Start workflow.
            var workflowInstance = await startsWorkflow.StartWorkflowAsync(workflowBlueprint);
        }
    }
}
