using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner.Components.StepExplorer;

partial class StepExplorer : ComponentBase
{
    [Inject] 
    private StepsViewModel StepsViewModel { get; set; } = default!;

    private string searchText;
    private bool hideUnmatched;

    protected override async Task OnInitializedAsync()
    {
        StepsViewModel.Initialize();

        await base.OnInitializedAsync();
    }
}
