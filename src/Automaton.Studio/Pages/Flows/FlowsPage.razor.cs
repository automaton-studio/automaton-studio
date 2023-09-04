using AntDesign;
using Automaton.Studio.Pages.Flows.Components.NewFlow;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Flows
{
    partial class FlowsPage : ComponentBase
    {
        public bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private FlowsViewModel FlowsViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = true;

            try
            {
                await FlowsViewModel.GetFlows();
                await FlowsViewModel.GetRunners();
            }
            catch
            {
                await MessageService.Error(Resources.Errors.FlowsListNotLoaded);
            }
            finally
            {
                loading = false;
            }

            await base.OnInitializedAsync();
        }

        private async Task RunFlow(FlowModel flow)
        {
            await FlowsViewModel.RunFlow(flow.Id, flow.RunnerIds);
        }

        private void EditFlow(Guid id)
        {
            NavigationManager.NavigateTo($"flowdesigner/{id}");
        }

        private async Task DeleteFlow(Guid id)
        {
            await FlowsViewModel.DeleteFlow(id);

            StateHasChanged();
        }

        private async Task NewFlowDialog()
        {
            var newFlowModel = new NewFlowModel();

            var modalRef = await ModalService.CreateModalAsync<NewFlowDialog, NewFlowModel>
            (
                new ModalOptions { Title = "New Flow" }, newFlowModel
            );

            modalRef.OnOk = async () =>
            {
                try
                {
                    await FlowsViewModel.CreateFlow(newFlowModel.Name);
                    StateHasChanged();
                }
                catch
                {
                    await MessageService.Error($"Flow {newFlowModel.Name} could not be created");
                }
            };
        }
    }
}
