using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Pages.Designer;
using Automaton.Studio.Pages.StepDesigner.Components;
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

        private int currentStepSection;
        private Type stepSection = typeof(StepDesignerProperties);
        private Dictionary<string, object> customStepParameters;
        private Dictionary<int, Type> stepSections;

        public StepDesignerPage()
        {
            stepSections = new()
            {
                { 1,  typeof(StepDesignerProperties) },
                { 2,  typeof(StepDesignerProperties) },
                { 3,  typeof(StepDesignerProperties) }
            };
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

        private void OnStepsChange(int stepSectionIndex)
        {
            currentStepSection = stepSectionIndex;

            NavigateToStepSection(currentStepSection);
        }

        private void NavigateToStepSection(int stepSectionIndex)
        {
            stepSection = stepSections[stepSectionIndex];
        }

        private async Task Save()
        {
            await StepDesignerViewModel.Save();
        }
    }
}
