using Automaton.Studio.Models;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IWorkflowsViewModel WorkflowsViewModel { get; set; } = default!;

        public class NewWorkflowModel
        {
            [Required]
            public string Name { get; set; }
        }

        private bool NewWorkflowVisible { get; set; }
        private NewWorkflowModel workflowModel = new NewWorkflowModel();
        AntDesign.Form<NewWorkflowModel> form;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await WorkflowsViewModel.Initialize();
        }

        private async Task RunWorkflow(WorkflowModel workflow)
        {
            await WorkflowsViewModel.RunWorkflow(workflow);
        }

        private void EditWorkflow(WorkflowModel workflow)
        {
            NavigationManager.NavigateTo($"designer/{workflow.DefinitionId}");
        }

        private void ShowNewWorkflowDialog()
        {
            NewWorkflowVisible = true;
        }

        private void NewWorkflowOk(MouseEventArgs e)
        {
            var result = form.Validate();

            if(result)
                NewWorkflowVisible = false;


        }

        private void NewWorkflowCancel(MouseEventArgs e)
        {
            NewWorkflowVisible = false;
        }
    }
}
