using AntDesign;
using Automaton.Studio.Components.Dialogs.WorkflowName;
using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class SolutionFlow : ComponentBase
    {
        #region Members

        private string searchText { get; set; }

        #endregion

        #region Properties

        [CascadingParameter]
        private string FlowId { get; set; }

        [Inject]
        private ISolutionFlowViewModel FlowViewModel { get; set; }

        [Inject]
        private ModalService ModalService { get; set; }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await FlowViewModel.LoadFlow(FlowId);

            await base.OnInitializedAsync();
        }

        #region Methods

        private async Task RenameWorkflow(WorkflowModel workflowModel)
        {
            var workflowNameModel = new WorkflowNameModel()
            {
                Name = workflowModel.Name,
                ExistingNames = FlowViewModel.WorkflowNames
            };

            var modalRef = await ModalService.CreateModalAsync<WorkflowNameDialog, WorkflowNameModel>
            (
                new ModalOptions { Title = Labels.RenameWorkflow }, 
                workflowNameModel
            );

            modalRef.OnOk = async () =>
            {
                FlowViewModel.RenameWorkflow(workflowModel.Id, workflowNameModel.Name);
                StateHasChanged();
            };  
        }

        private async Task OnSearchTextChange(string value)
        {
            throw new NotImplementedException();
        }

        private async Task OnSearch()
        {
            throw new NotImplementedException();
        }

        private async Task OnEnter(KeyboardEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
