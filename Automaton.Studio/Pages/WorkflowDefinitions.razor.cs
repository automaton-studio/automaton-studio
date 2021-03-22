using Automaton.Studio.Activities;
using ElsaDashboard.Shared.Rpc;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class WorkflowDefinitions
    {
        protected IList<ActivityBase> activities;

        [Inject] private IWorkflowDefinitionService WorkflowDefinitionService { get; set; } = default!;
        [Inject] private IActivityService ActivityService { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            //var workflowDefinitions = await WorkflowDefinitionService.ListAsync();

            activities = new List<ActivityBase>
            {
                new ConsoleActivity()
                {
                    Id = 1,
                    Name = "Console Activity"
                }
            };
        }
    }
}
