using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Automaton.Studio.Hubs
{
    public class WorkflowHub : Hub
    {
        public async Task RunWorkflow(string definitionId)
        {
            await Clients.All.SendAsync("RunWorkflow", definitionId);
        }
    }
}
