using AntDesign;
using Automaton.Studio.Components;
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
        [Inject] private ModalService ModalService { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitializedAsync();

            FlowsViewModel.Initialize();
        }

        private async Task RunWorkflow(FlowModel flow)
        {
            await FlowsViewModel.RunWorkflow(flow);
        }

        private void EditWorkflow(FlowModel flow)
        {
            NavigationManager.NavigateTo($"designer/{flow.Id}");
        }

        private void DeleteWorkflow(FlowModel flow)
        {
            FlowsViewModel.DeleteFlow(flow);
            //StateHasChanged();
        }

        private async Task ShowNewWorkflowDialog()
        {
            var modalConfig = new ModalOptions
            {
                Title = Labels.NewFlowTitle,
                // Needed as a workaround to prevent dialog
                // close imediatelly when clicking OK button
                MaskClosable = false
            };

            var modalRef = await ModalService.CreateModalAsync<NewFlow, FlowModel>(modalConfig, FlowsViewModel.NewFlow);

            modalRef.OnOk = async () =>
            {
                // Needed to update OK button loading icon
                modalRef.Config.ConfirmLoading = true;
                await modalRef.UpdateConfigAsync();

                var flow = await FlowsViewModel.CreateNewFlow();

                NavigationManager.NavigateTo($"designer/{flow.Id}");
            };
        }
    }
}
