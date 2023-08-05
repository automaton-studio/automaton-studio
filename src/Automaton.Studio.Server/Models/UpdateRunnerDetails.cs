using MediatR;

namespace Automaton.Studio.Server.Models
{
    public class UpdateRunnerDetails : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
