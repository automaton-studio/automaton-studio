using MediatR;

namespace Automaton.Studio.Server.Models
{
    public class RegisterRunnerRequest : IRequest
    {
        public string Name { get; set; }
    }
}
