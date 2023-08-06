using MediatR;

namespace Automaton.Studio.Server.Models
{
    public class RunnerDetails : IRequest
    {
        public string Name { get; set; }
    }
}
