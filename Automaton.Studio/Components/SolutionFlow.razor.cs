using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class SolutionFlow : ComponentBase
    {
        [Inject] 
        private ISolutionFlowViewModel FlowViewModel { get; set; } = default!;

        [CascadingParameter]
        protected string FlowId { get; set; }

        private string searchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await FlowViewModel.LoadFlow(FlowId);

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
