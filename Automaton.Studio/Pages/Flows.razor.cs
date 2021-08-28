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

            FlowsViewModel.LoadFlows();
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
            FlowsViewModel.DeleteFlow(flow.Id);
        }

        private async Task ShowNewWorkflowDialog()
        {
            await FlowsViewModel.CreateFlow();
        }
    }
}
