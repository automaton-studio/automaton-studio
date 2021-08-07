using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class WorkflowsTree : ComponentBase
    {
        [Inject] 
        private ITreeWorkflowViewModel TreeWorkflowViewModel { get; set; } = default!;

        private string searchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            TreeWorkflowViewModel.Initialize();

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
