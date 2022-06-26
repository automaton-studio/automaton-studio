using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class ExecuteFlowCommandHandler : IRequestHandler<ExecuteFlowCommand>
    {
        private readonly RunnerService runnerService;

        public ExecuteFlowCommandHandler(RunnerService runnerService)
        {
            this.runnerService = runnerService;
        }

        public async Task<Unit> Handle(ExecuteFlowCommand command, CancellationToken cancellationToken)
        {
            await runnerService.RunFlow(command.FlowId, command.RunnerIds, cancellationToken);

            return Unit.Value;
        }
    }
}