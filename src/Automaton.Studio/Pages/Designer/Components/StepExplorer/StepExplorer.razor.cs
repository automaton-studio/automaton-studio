using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.Designer.Components.StepExplorer;

partial class StepExplorer : ComponentBase
{
    [Inject] 
    private StepsViewModel StepsViewModel { get; set; } = default!;

    private string searchText { get; set; }

    protected override async Task OnInitializedAsync()
    {
        StepsViewModel.Initialize();

        await base.OnInitializedAsync();
    }

    private void OnSearchChange(string text)
    {
        throw new NotImplementedException();
    }

    public async Task OnSearch()
    {
        throw new NotImplementedException();
    }

    private async Task OnEnter(KeyboardEventArgs e)
    {
        throw new NotImplementedException();
    }
}
