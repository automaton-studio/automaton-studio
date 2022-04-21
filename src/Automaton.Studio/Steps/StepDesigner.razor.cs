using AntDesign;
using Automaton.Studio.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps
{
    public partial class StepDesigner : ComponentBase
    {
        [Parameter]
        public Domain.Step Step { get; set; }

        [Parameter] 
        public RenderFragment ChildContent { get; set; }

        [Inject] 
        private ModalService ModalService { get; set; } = default!;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        #region Event Handlers

        private async Task OnEdit(Domain.Step step)
        {
            var result = await step.DisplayPropertiesDialog(ModalService);

            result.OnOk = () => {

                StateHasChanged();

                return Task.CompletedTask;
            };
        }

        private static void OnDelete(Domain.Step step)
        {
            step.Definition.DeleteStep(step);
        }

        #endregion
    }
}
