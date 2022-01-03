using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Components
{
    partial class SolutionActivities : ComponentBase
    {
        [Inject] 
        private ISolutionActivitiesViewModel ActivitiesViewModel { get; set; } = default!;

        private string searchText { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ActivitiesViewModel.Initialize();

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
}
