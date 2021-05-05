using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await DesignerViewModel.Initialize();
        }
    }
}
