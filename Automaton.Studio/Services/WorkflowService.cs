using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Factories;
using Elsa.Models;
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
        private readonly ActivityFactory activityFactory;
        private readonly IMapper mapper;

        public WorkflowService(
            IWorkflowBlueprintMaterializer workflowBlueprintMaterializer,
            IWorkflowDefinitionStore workflowDefinitionStore,
            IStartsWorkflow startsWorkflow,
            ActivityFactory activityFactory,
            IMapper mapper)
        {
            this.workflowDefinitionStore = workflowDefinitionStore;
            this.workflowBlueprintMaterializer = workflowBlueprintMaterializer;
            this.startsWorkflow = startsWorkflow;
            this.activityFactory = activityFactory;
            this.mapper = mapper;
        }

        /// <summary>
        /// Runs workflow by definition id
        /// </summary>
        /// <param name="workflowId">Workflow definition id</param>
        public async Task RunWorkflow(string workflowId)
        {
            // Retrieve workflow definition from store.
            var workflowDefinition = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(workflowId));

            // Create blueprint from wokflow definition.
            var workflowBlueprint = await workflowBlueprintMaterializer.CreateWorkflowBlueprintAsync(workflowDefinition);

            // Start workflow.
            await startsWorkflow.StartWorkflowAsync(workflowBlueprint);
        }

        /// <summary>
        /// Runs workflow from memory
        /// </summary>
        /// <param name="studioWorkflow">Studio workflow</param>
        public async Task RunWorkflow(StudioWorkflow studioWorkflow)
        {
            var workflowDefinition = new WorkflowDefinition();

            // Update WorkflowDefinition with details from StudioWorkflow
            mapper.Map(studioWorkflow, workflowDefinition);

            // Create blueprint from wokflow definition.
            var workflowBlueprint = await workflowBlueprintMaterializer.CreateWorkflowBlueprintAsync(workflowDefinition);

            // Start workflow.
            await startsWorkflow.StartWorkflowAsync(workflowBlueprint);
        }

        /// <summary>
        /// Load workflow from database
        /// </summary>
        /// <param name="workflowId">Workflow identifier</param>
        public async Task<StudioWorkflow> LoadWorkflow(string workflowId)
        {
            // Find WorkflowDefinition workflow based on workflow id
            var workflowDefinition = await workflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(workflowId));

            // Map WorkflowDefinition to StudioWorkflow
            var studioWorkflow = mapper.Map<WorkflowDefinition, StudioWorkflow>(workflowDefinition);

            // Elsa to Studio activities are not easily mapped, so we are doing it separately
            foreach (var activityDefinition in workflowDefinition.Activities)
            {
                var studioActivity = activityFactory.GetStudioActivity(activityDefinition);
                studioWorkflow.LoadActivity(studioActivity);
            }

            return studioWorkflow;
        }

        /// <summary>
        /// Save workflow to database
        /// </summary>
        public async Task SaveWorkflow(StudioWorkflow studioWorkflow)
        {
            var workflowDefinition = new WorkflowDefinition();

            // Update WorkflowDefinition with details from StudioWorkflow
            mapper.Map(studioWorkflow, workflowDefinition);

            await workflowDefinitionStore.AddAsync(workflowDefinition);
            await workflowDefinitionStore.SaveAsync(workflowDefinition);

            studioWorkflow.Id = workflowDefinition.Id;
        }
    }
}
