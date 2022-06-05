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
        private readonly IHubContext<WorkflowHub> workflowHubContext;

        public ExecuteFlowCommandHandler(RunnerService runnerService, IHubContext<WorkflowHub> workflowHubContext)
        {
            this.runnerService = runnerService;
            this.workflowHubContext = workflowHubContext;
        }

        public async Task<Unit> Handle(ExecuteFlowCommand command, CancellationToken cancellationToken)
        {
            var runners = await runnerService.List(command.RunnerIds, cancellationToken);

            foreach (var runner in runners)
            {
                var client = workflowHubContext.Clients.Client(runner.ConnectionId);

                //await workflowHubContext.Clients.All.SendAsync("RunWorkflow", command.FlowId);

                try
                {
                    await client.SendAsync("RunWorkflow", command.FlowId);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return Unit.Value;
        }
    }
}