using MediatR;

namespace Automaton.Studio.Server.Models
{
    public class RegisterRunnerDetails : IRequest
    {
        public string Name { get; set; }
    }
}
