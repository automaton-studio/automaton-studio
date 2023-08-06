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

        TypographyEditableConfig runnerNameEditableConfig;

        protected override async Task OnInitializedAsync()
        {
            if (!ConfigService.IsRunnerRegistered())
            {
                NavigationManager.NavigateTo($"/setup");
                return;
            }

            await DashboardViewModel.ConnectHub();

            runnerNameEditableConfig = new TypographyEditableConfig
            {
                OnChange = OnNameChanged,
                Text = DashboardViewModel.RunnerName
            };
        }

        private void OnNameChanged(string name)
        {
            Task.Run(async () => await DashboardViewModel.UpdateRunnerName(name)).Wait();
        }
    }
}
