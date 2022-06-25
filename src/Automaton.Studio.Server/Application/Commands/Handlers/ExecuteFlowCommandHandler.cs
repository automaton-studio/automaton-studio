using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Hubs;
using Automaton.Studio.Server.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class ExecuteFlowCommandHandler : IRequestHandler<ExecuteFlowCommand>
    {
        private readonly RunnerService runnerService;
        private readonly IHubContext<AutomatonHub> automatonHub;

        public ExecuteFlowCommandHandler(RunnerService runnerService, IHubContext<AutomatonHub> automatonHub)
        {
            this.runnerService = runnerService;
            this.automatonHub = automatonHub;
        }

        public async Task<Unit> Handle(ExecuteFlowCommand command, CancellationToken cancellationToken)
        {
            var runners = await runnerService.List(command.RunnerIds, cancellationToken);

            foreach (var runner in runners)
            {
                var client = automatonHub.Clients.Client(runner.ConnectionId);

                await client.SendAsync(AutomatonHubMethods.RunWorkflow, command.FlowId);
            }

            return Unit.Value;
        }
    }
}