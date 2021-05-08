using AntDesign;
using Automaton.Studio.Forms;
using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Workflows : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IWorkflowsViewModel WorkflowsViewModel { get; set; } = default!;
        [Inject] ModalService ModalService { get; set; } = default!;

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

        private async Task ShowNewWorkflowDialog()
        {
            var modalConfig = new ModalOptions
            {
                Title = Labels.NewWorkflowTitle
            };

            var modalRef = await ModalService.CreateModalAsync<NewWorkflowForm, NewWorkflowModel>(modalConfig, WorkflowsViewModel.NewWorkflowDetails);

            modalRef.OnOk = () =>
            {
                var a = WorkflowsViewModel.NewWorkflowDetails;
                return Task.CompletedTask;
            };
        }
    }
}
