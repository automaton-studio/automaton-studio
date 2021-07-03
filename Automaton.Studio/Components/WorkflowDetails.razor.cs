
using Microsoft.AspNetCore.Components;
using Elsa.Persistence;
using AntDesign;
using Automaton.Studio.Activity;
using Blazored.FluentValidation;
using System.Threading.Tasks;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using AutoMapper;

namespace Automaton.Studio.Components
{
    public partial class WorkflowDetails
    {
        #region Private methods

        private StudioWorkflow studioWorkflow;
        private FluentValidationValidator fluentValidationValidator;

        #endregion

        #region Injection

        [Inject]
        private IWorkflowDefinitionStore WorkflowDefinitionStore { get; set; }

        [Inject]
        private IMapper Mapper { get; set; }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            studioWorkflow = this.Options;
        }

        public async Task Submit()
        {
            if (fluentValidationValidator.Validate(options => options.IncludeAllRuleSets()))
            {
                // Find Elsa workflow based on workflow id
                var elsaWorkflow = await WorkflowDefinitionStore.FindAsync(new WorkflowDefinitionIdSpecification(studioWorkflow.DefinitionId));

                if (elsaWorkflow != null)
                {
                    // Update ElsaWorkflow with details from StudioWorkflow
                    Mapper.Map(studioWorkflow, elsaWorkflow);

                    // Save Elsa workflow
                    await WorkflowDefinitionStore.SaveAsync(elsaWorkflow);
                }

                // Close drawer and return true
                var drawerRef = base.FeedbackRef as DrawerRef<bool>;
                await drawerRef!.CloseAsync(true);
            }
        }

        public async Task Cancel()
        {
            // Close drawer
            await CloseFeedbackAsync();
        }
    }
}
