using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class ActivitiesTree : ComponentBase
    {
        [Inject] 
        private ITreeActivityViewModel TreeActivityViewModel { get; set; } = default!;

        private string searchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            TreeActivityViewModel.Initialize();

            await base.OnInitializedAsync();
        }

        private void Handle(string value)
        {
        }

        public async Task OnSearch()
        {
            //searchText
        }

        private async Task OnEnter(KeyboardEventArgs e)
        {
        }
    }
}
