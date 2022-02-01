using AntDesign;
using Automaton.Studio.Components.NewDefinition;
using Automaton.Studio.Domain;
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
        private Flow Flow { get; set; }

        [Inject]
        private IFlowExplorerViewModel FlowExplorerViewModel { get; set; }

        [Inject]
        private ModalService ModalService { get; set; }

        #endregion

        protected override void OnInitialized()
        {
            base.OnInitializedAsync();
        }

        #region Methods

        public void UpdateState()
        {
            StateHasChanged();
        }

        private async Task RenameWorkflow(FlowExplorerDefinition explorerDefinition)
        {
            var definitionModel = new NewDefinitionModel()
            {
                Name = explorerDefinition.Name,
                ExistingNames = FlowExplorerViewModel.DefinitionNames
            };

            var modalRef = await ModalService.CreateModalAsync<NewDefinitionDialog, NewDefinitionModel>
            (
                new ModalOptions { Title = Labels.RenameWorkflow }, 
                definitionModel
            );

            modalRef.OnOk = () =>
            {
                FlowExplorerViewModel.RenameDefinition(explorerDefinition.Id, definitionModel.Name);
                StateHasChanged();

                return Task.CompletedTask;
            };  
        }

        private string GetClassForDefinition(FlowExplorerDefinition explorerDefinition)
        {
            return explorerDefinition.IsStartup ? "selected-definition" : string.Empty;
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
