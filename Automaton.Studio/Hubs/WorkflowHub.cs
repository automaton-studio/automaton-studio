using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Hubs
{
    public class WorkflowHub : Hub
    {
        public async Task RunWorkflow(string definitionId)
        {
            await Clients.All.SendAsync("RunWorkflow", definitionId);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("WelcomeRunner", Context.User.Identity.Name);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
