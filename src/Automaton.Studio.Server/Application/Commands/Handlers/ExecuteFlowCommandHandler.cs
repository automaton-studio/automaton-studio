using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class ExecuteFlowCommandHandler : IRequestHandler<ExecuteFlowCommand>
    {
        private readonly FlowsService flowService;

        public ExecuteFlowCommandHandler(FlowsService flowService)
        {
            this.flowService = flowService;
        }

        public async Task<Unit> Handle(ExecuteFlowCommand command, CancellationToken cancellationToken)
        {
            await flowService.Execute(command.FlowId, command.RunnerIds, cancellationToken);

            return Unit.Value;
        }
    }
}