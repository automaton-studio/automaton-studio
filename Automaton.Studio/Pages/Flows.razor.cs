using AntDesign;
using Automaton.Studio.Components.Dialogs.NewFlow;
using Automaton.Studio.Models;
using Automaton.Studio.Resources;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Flows : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IFlowsViewModel FlowsViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitializedAsync();
        }

        private async Task RunFlow(FlowModel flow)
        {
            await FlowsViewModel.RunFlow(flow);
        }

        private void EditFlow(FlowModel flow)
        {
            NavigationManager.NavigateTo($"flow/{flow.Id}");
        }

        private void DeleteFlow(FlowModel flow)
        {
            FlowsViewModel.DeleteFlow(flow.Id);
        }

        private async Task NewFlowDialog()
        {
            var flowModel = new FlowModel();
            var modalRef = await ModalService.CreateModalAsync<NewFlowDialog, FlowModel>
            (
                new ModalOptions
                {
                    Title = Labels.NewFlowTitle
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
                    await MessageService.Error(Errors.NewFlowError);
                }
            };
        }
    }
}
