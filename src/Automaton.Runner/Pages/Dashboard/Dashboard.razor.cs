using AntDesign;
using Automaton.Runner.Storage;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Dashboard
{
    public partial class Dashboard : ComponentBase
    {
        [Inject] Services.ConfigService ConfigService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] DashboardViewModel DashboardViewModel { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (!ConfigService.IsRunnerRegistered())
            {
                NavigationManager.NavigateTo($"/setup");
                return;
            }

            await DashboardViewModel.ConnectHub();
        }

        private void OnNameChanged(string name)
        {
            DashboardViewModel.RunnerName = name;
        }
    }
}
