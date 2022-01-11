using AntDesign;
using Automaton.Studio.Components;
using Automaton.Studio.Models;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Flows : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IDefinitionsViewModel FlowsViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        private IEnumerable<DefinitionModel> Definitions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Definitions = await FlowsViewModel.GetDefinitions();

            base.OnInitializedAsync();
        }

        private async Task RunFlow(DefinitionModel flow)
        {
            await FlowsViewModel.RunFlow(flow);
        }

        private void EditFlow(DefinitionModel flow)
        {
            NavigationManager.NavigateTo($"flow/{flow.Id}");
        }

        private void DeleteFlow(DefinitionModel flow)
        {
            FlowsViewModel.DeleteFlow(flow.Id);
        }

        private async Task NewFlowDialog()
        {
            var flowModel = new NewFlowModel();
            var modalRef = await ModalService.CreateModalAsync<NewFlowDialog, NewFlowModel>
            (
                new ModalOptions
                {
                    Title = "New Flow"
                },
                flowModel
            );

            modalRef.OnOk = async () =>
            {
                try
                {
                    await FlowsViewModel.CreateFlow(flowModel.Name);
                    StateHasChanged();
                }
                catch
                {
                    await MessageService.Error("Error");
                }
            };
        }
    }
}
