using Automaton.Studio.Server.Core.Commands;
using Automaton.Studio.Server.Services;
using MediatR;

namespace Automaton.Studio.Server.Application.Commands.Handlers
{
    public class RegisterRunnerCommandHandler : IRequestHandler<RegisterRunnerCommand>
    {
        private readonly RunnerService runnerService;

        public RegisterRunnerCommandHandler(RunnerService runnerService)
        {
            this.runnerService = runnerService;
        }

        public async Task<Unit> Handle(RegisterRunnerCommand command, CancellationToken cancellationToken)
        {
            await runnerService.Create(command.Name, cancellationToken);

            return Unit.Value;
        }
    }
}
