using Elsa.Persistence;
using Elsa.Services;
using System.Threading.Tasks;

namespace Automaton.Runner.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowRunner workflowRunner;
        private readonly IWorkflowDefinitionStore workflowDefinitionStore;
        private readonly IWorkflowBlueprintMaterializer workflowBlueprintMaterializer;
        private readonly IBuildsAndStartsWorkflow buildsAndStartsWorkflow;
        private readonly IStartsWorkflow startsWorkflow;
        

        public WorkflowService(IWorkflowRunner workflowRunner,
            IWorkflowBlueprintMaterializer workflowBlueprintMaterializer,
            IWorkflowDefinitionStore workflowDefinitionStore,
            IBuildsAndStartsWorkflow buildsAndStartsWorkflow,
            IStartsWorkflow startsWorkflow)
        {
            this.workflowRunner = workflowRunner;
            this.workflowDefinitionStore = workflowDefinitionStore;
            this.workflowBlueprintMaterializer = workflowBlueprintMaterializer;
            this.buildsAndStartsWorkflow = buildsAndStartsWorkflow;
            this.startsWorkflow = startsWorkflow;
        }

        public async Task RunWorkflow(string workflowId)
        {
            // Retrieve workflow definition from store.
            var storeWorkflowDefinition = await workflowDefinitionStore.FindAsync(new Elsa.Persistence.Specifications.WorkflowDefinitions.WorkflowDefinitionIdSpecification(workflowId));

            // Create blueprint from wokflow definition.
            var workflowBlueprint = await workflowBlueprintMaterializer.CreateWorkflowBlueprintAsync(storeWorkflowDefinition);

            // Run the workflow.
             var workflowInstance = await startsWorkflow.StartWorkflowAsync(workflowBlueprint);
        }
    }
}
