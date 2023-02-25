using AntDesign;
using Automaton.Studio.Pages.Flows.Components.NewFlow;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomSteps
{
    partial class CustomStepsPage : ComponentBase
    {
        public bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private CustomStepsViewModel CustomStepsViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = true;

            try
            {
                await CustomStepsViewModel.GetCustomSteps();
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

        private void EditFlow(Guid id)
        {
            NavigationManager.NavigateTo($"customstepdesigner/{id}");
        }

        private async Task DeleteFlow(Guid id)
        {
            await CustomStepsViewModel.DeleteFlow(id);

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
                    await CustomStepsViewModel.CreateCustomStep(newFlowModel.Name);
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
