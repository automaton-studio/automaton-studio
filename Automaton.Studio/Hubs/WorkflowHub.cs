using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Automaton.Studio.Hubs
{
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class WorkflowHub : Hub
    {
        private const string RunnerNameHeader = "RunnerName";

        public override async Task OnConnectedAsync()
        {
            var httpCtx = Context.GetHttpContext();
            var runnerName = httpCtx.Request.Headers[RunnerNameHeader].ToString();

            await Clients.Caller.SendAsync("WelcomeRunner", $"Welcome {Context.User.Identity.Name} and Runner {runnerName}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
