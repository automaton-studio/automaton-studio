using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;

        [Parameter] public string WorkflowId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            
            if (!string.IsNullOrEmpty(WorkflowId))
            {
                await DesignerViewModel.LoadWorkflow(WorkflowId);
            }
        }
    }
}
