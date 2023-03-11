using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Pages.StepDesigner.Properties;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.StepDesigner
{
    partial class StepDesignerPage : ComponentBase
    {
        [Parameter] public string StepId { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private StepDesignerViewModel StepDesignerViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        private DynamicComponent? stepDesignerComponent;
        private Type stepSection = typeof(StepDesignerProperties);
        private Dictionary<string, object> customStepParameters;

        private bool loading = false;
        private Form<CustomStep> form;
        private FluentValidationValidator fluentValidationValidator;

        public StepDesignerPage()
        {          
        }

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
            stepSection = typeof(StepDesignerProperties);
        }

        private void NavigateToCode()
        {
            stepSection = typeof(StepDesignerProperties);
        }

        private async Task Save()
        {
            await StepDesignerViewModel.Save();
        }
    }
}
