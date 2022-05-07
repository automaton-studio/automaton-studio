using MediatR;

namespace Automaton.Studio.Server.Core.Commands
{
    public class RegisterRunnerCommand : IRequest
    {
        public string Name { get; set; }
    }
}
