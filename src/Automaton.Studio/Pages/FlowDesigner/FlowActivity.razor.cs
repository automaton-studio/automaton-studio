using AntDesign;
using Automaton.Studio.Pages.Runners;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.FlowDesigner
{
    public partial class FlowActivity : ComponentBase
    {
        [Parameter] public string FlowId { get; set; }

        public bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private FlowActivityViewModel FlowActivityViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = true;

            try
            {
                await FlowActivityViewModel.GetFlowActivity();
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
    }
}
