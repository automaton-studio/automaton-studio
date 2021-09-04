using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class SolutionFlow : ComponentBase
    {
        #region Members

        private string searchText { get; set; }

        #endregion

        #region Properties

        [CascadingParameter]
        private string FlowId { get; set; }

        [Inject]
        private ISolutionFlowViewModel FlowViewModel { get; set; } = default!;

        #endregion

        protected override async Task OnInitializedAsync()
        {
            await FlowViewModel.LoadFlow(FlowId);

            await base.OnInitializedAsync();
        }

        #region Methods

        private void Handle(string value)
        {
        }

        private async Task OnSearch()
        {
            throw new NotImplementedException();
        }

        private async Task OnEnter(KeyboardEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
