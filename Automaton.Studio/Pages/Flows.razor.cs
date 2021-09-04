using Automaton.Studio.Models;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Flows : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private IFlowsViewModel FlowsViewModel { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitializedAsync();
        }

        private async Task RunFlow(FlowModel flow)
        {
            await FlowsViewModel.RunWorkflow(flow);
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
            await FlowsViewModel.CreateFlow();
        }
    }
}
