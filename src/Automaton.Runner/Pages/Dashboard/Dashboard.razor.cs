using Automaton.Core.Events;
using Microsoft.AspNetCore.Components;

namespace Automaton.Runner.Pages.Dashboard
{
    public partial class Dashboard : ComponentBase, IDisposable
    {
        [Inject] ICourier Courier { get; set; }
        [Inject] Services.ConfigurationService ConfigService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] DashboardViewModel DashboardViewModel { get; set; }

        public async Task HandleHubConnectionNotification(HubConnectionNotification notification, CancellationToken cancellationToken)
        {
            DashboardViewModel.SetHubConnection(notification.HubConnectionState);

            await InvokeAsync(StateHasChanged);
        }

        protected override async Task OnInitializedAsync()
        {
            Courier.Subscribe<HubConnectionNotification>(HandleHubConnectionNotification);

            if (!ConfigService.IsRunnerRegistered())
            {
                NavigationManager.NavigateTo($"/setup");
                return;
            }

            if (!DashboardViewModel.IsRunnerConnected())
                await DashboardViewModel.ConnectHub();
        }

        public void Dispose()
        {
            Courier.UnSubscribe<HubConnectionNotification>(HandleHubConnectionNotification);
        }
    }
}
