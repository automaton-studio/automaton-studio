using AntDesign;
using Automaton.Studio.Components.NewDefinition;
using Automaton.Studio.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components.Explorer.FlowExplorer
{
    partial class FlowExplorer : ComponentBase
    {
        #region Members

        private string searchText { get; set; }

        #endregion

        #region Properties

        [CascadingParameter]
        private string FlowId { get; set; }

        [Inject]
        private IFlowExplorerViewModel FlowExplorerViewModel { get; set; }

        [Inject]
        private ModalService ModalService { get; set; }

        #endregion

        protected override async Task OnInitializedAsync()
        {
            if (!string.IsNullOrEmpty(FlowId))
            {
                await FlowExplorerViewModel.LoadFlow(FlowId); 
            }

            await base.OnInitializedAsync();
        }

        #region Methods

        public void UpdateState()
        {
            StateHasChanged();
        }

        private async Task RenameWorkflow(FlowExplorerDefinition workflowModel)
        {
            var workflowNameModel = new NewDefinitionModel()
            {
                Name = workflowModel.Name,
                ExistingNames = FlowExplorerViewModel.DefinitionNames
            };

            var modalRef = await ModalService.CreateModalAsync<NewDefinitionDialog, NewDefinitionModel>
            (
                new ModalOptions { Title = Labels.RenameWorkflow }, 
                workflowNameModel
            );

            modalRef.OnOk = () =>
            {
                FlowExplorerViewModel.RenameWorkflow(workflowModel.Id, workflowNameModel.Name);
                StateHasChanged();

                return Task.CompletedTask;
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
