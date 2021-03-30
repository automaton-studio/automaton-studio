using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Hubs
{
    public class WorkflowHub : Hub
    {
        private readonly AuthenticationStateProvider authenticationStateProvider;

        public WorkflowHub(AuthenticationStateProvider authenticationStateProvider)
        {
            this.authenticationStateProvider = authenticationStateProvider;
        }

        public async Task RunWorkflow(string definitionId)
        {
            await Clients.All.SendAsync("RunWorkflow", definitionId);
        }

        public override async Task OnConnectedAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            await Groups.AddToGroupAsync(Context.ConnectionId, user.Identity.Name);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Identity.Name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
