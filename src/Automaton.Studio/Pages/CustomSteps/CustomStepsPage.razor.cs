using AntDesign;
using Automaton.Studio.Pages.CustomSteps.Components.NewCustomStep;
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
                await MessageService.Error(Resources.Errors.CustomStepsListNotLoaded);
            }
            finally
            {
                loading = false;
            }

            await base.OnInitializedAsync();
        }

        private void EditCustomStep(Guid id)
        {
            NavigationManager.NavigateTo($"customstepdesigner/{id}");
        }

        private async Task DeleteCustomStep(Guid id)
        {
            await CustomStepsViewModel.DeleteCustomStep(id);

            StateHasChanged();
        }

        private async Task NewCustomStepDialog()
        {
            var model = new NewCustomStepModel();

            var modalRef = await ModalService.CreateModalAsync<NewCustomStepDialog, NewCustomStepModel>
            (
                new ModalOptions { Title = "New Custom Step" }, model
            );

            modalRef.OnOk = async () =>
            {
                try
                {
                    await CustomStepsViewModel.CreateCustomStep(model.Name);
                    StateHasChanged();
                }
                catch
                {
                    await MessageService.Error($"Custom step {model.Name} could not be created");
                }
            };
        }
    }
}
