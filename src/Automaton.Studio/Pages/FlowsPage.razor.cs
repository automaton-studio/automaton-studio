using AntDesign;
using Automaton.Studio.Components.NewFlow;
using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
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
        [Inject] public INavMenuService NavMenuService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await FlowsViewModel.GetFlows();

            await base.OnInitializedAsync();
        }

        private async Task RunFlow(FlowModel flow)
        {
            await FlowsViewModel.RunFlow(flow);
        }

        private void EditFlow(FlowModel flow)
        {
            NavMenuService.EnableDesignerMenu();
            NavigationManager.NavigateTo($"designer/{flow.Id}");
        }

        private async Task DeleteFlow(FlowModel flow)
        {
            await FlowsViewModel.DeleteFlow(flow);
            StateHasChanged();
        }

        private async Task NewFlowDialog()
        {
            var newFlowModel = new NewFlowModel();
            var modalRef = await ModalService.CreateModalAsync<NewFlowDialog, NewFlowModel>
            (
                new ModalOptions
                {
                    Title = "New Flow"
                },
                newFlowModel
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
