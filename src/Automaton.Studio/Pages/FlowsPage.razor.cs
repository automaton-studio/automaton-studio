using AntDesign;
using Automaton.Studio.Components.NewFlow;
using Automaton.Studio.Models;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class FlowsPage : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IFlowViewModel FlowsViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        private IEnumerable<FlowModel> Flows { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Flows = await FlowsViewModel.GetFlows();

            await base.OnInitializedAsync();
        }

        private async Task RunFlow(FlowModel flow)
        {
            await FlowsViewModel.RunFlow(flow);
        }

        private void EditFlow(FlowModel flow)
        {
            NavigationManager.NavigateTo($"designer/{flow.Id}");
        }

        private void DeleteFlow(FlowModel flow)
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
