using Automaton.Core.Events;
using MediatR;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Automaton.Runner.Pages.Dashboard
{
    public partial class Dashboard : ComponentBase, INotificationHandler<HubConnectionNotification>, IDisposable
    {
        private static event EventHandler<HubConnectionNotification> HubConnectionChange;

        [Inject] Services.ConfigurationService ConfigService { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] DashboardViewModel DashboardViewModel { get; set; }

        public Task Handle(HubConnectionNotification notification, CancellationToken cancellationToken)
        {
            HubConnectionChange?.Invoke(this, new HubConnectionNotification(notification.HubConnectionState));

            return Task.CompletedTask;
        }

        protected override async Task OnInitializedAsync()
        {
            if (!ConfigService.IsRunnerRegistered())
            {
                NavigationManager.NavigateTo($"/setup");
                return;
            }

            HubConnectionChange += OnHubConnectionChange;

            if (!DashboardViewModel.IsRunnerConnected())
                await DashboardViewModel.ConnectHub();
        }

        private void OnHubConnectionChange(object? sender, HubConnectionNotification e)
        {
            DashboardViewModel.SetHubConnection(e.HubConnectionState);

            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            HubConnectionChange -= OnHubConnectionChange;
        }
    }
}
