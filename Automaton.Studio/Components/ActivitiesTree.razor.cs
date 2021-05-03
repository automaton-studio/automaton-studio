using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class ActivitiesTree : ComponentBase
    {
        [Inject] private ITreeActivityViewModel ActivitiesViewModel { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {
            await ActivitiesViewModel.Initialize();

        }
    }
}
