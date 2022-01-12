using AntDesign;
using Automaton.Studio.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    public partial class StepComponent : ComponentBase
    {
        [Parameter]
        public Conductor.Step Step { get; set; }

        [Parameter] 
        public RenderFragment ChildContent { get; set; }

        [Inject] 
        private ModalService ModalService { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        #region Event Handlers

        private async Task OnEdit(Conductor.Step step)
        {
            var result = await step.DisplayStepDialog(ModalService);

            result.OnOk = () => {

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        private static void OnDelete(Conductor.Step step)
        {
            step.StudioWorkflow.DeleteStep(step);
        }

        #endregion
    }
}
