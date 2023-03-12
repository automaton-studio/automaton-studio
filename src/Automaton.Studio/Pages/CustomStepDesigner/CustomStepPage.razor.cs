using AntDesign;
using Automaton.Studio.Domain;
using Microsoft.AspNetCore.Components;
using System.Threading;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.CustomStepDesigner
{
    partial class CustomStepPage : ComponentBase
    {
        private bool loading = false;
        private Form<CustomStep> form;
        private DynamicComponent? stepDesignerComponent;
        private Type stepSection = typeof(CustomStepProperties);
        private Dictionary<string, object> customStepParameters;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private CustomStepViewModel StepDesignerViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        [Parameter] public string StepId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await StepDesignerViewModel.Load(Guid.Parse(StepId));

            customStepParameters = new()
            {
                { nameof(CustomStep), StepDesignerViewModel.CustomStep }
            };

            await base.OnInitializedAsync();
        }

        private void NavigateToDetails()
        {
            stepSection = typeof(CustomStepProperties);
        }

        private void NavigateToCode()
        {
            stepSection = typeof(CustomStepCode);
        }

        private async Task Save()
        {
            try
            {
                loading = true;

                if (form.Validate())
                {
                    await StepDesignerViewModel.Save();
                }
            }
            catch (Exception ex)
            {
                await MessageService.Error(Resources.Errors.CustomStepUpdateFailed);
            }
            finally
            {
                loading = false;
            }
        }
    }
}
