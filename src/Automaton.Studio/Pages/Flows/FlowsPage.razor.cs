using AntDesign;
using Automaton.Core.Enums;
using Automaton.Studio.Models;
using Automaton.Studio.Pages.Flows.Components.NewFlow;
using Automaton.Studio.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Text;
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
            var results = await FlowsViewModel.RunFlow(flow.Id, flow.RunnerIds);

            var resultMessage = GetFlowExecutionResult(results);

            await MessageService.Info(resultMessage);
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

        private string GetFlowExecutionResult(IEnumerable<RunnerFlowResult> results)
        {
            var resultText = new StringBuilder();

            foreach (var result in results)
            {
                var runner = FlowsViewModel.Runners.SingleOrDefault(x => x.Id == result.RunnerId);

                var runnerMessage = result.Status == WorkflowStatus.None ?
                    $"Runner {runner.Name} did not respond" :
                    $"Runner {runner.Name} returned {result.Status}";

                resultText.AppendLine(runnerMessage);
            }

            return resultText.ToString();
        }
    }
}
