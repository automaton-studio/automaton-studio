using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class FlowTree : ComponentBase
    {
        [Inject] 
        private ITreeFlowViewModel TreeFlowViewModel { get; set; } = default!;

        private string searchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            TreeFlowViewModel.Initialize();

            await base.OnInitializedAsync();
        }

        private void Handle(string value)
        {
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
}
