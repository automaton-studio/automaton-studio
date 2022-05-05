#nullable disable

namespace Automaton.Studio.Server.Entities
{
    public class Runner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ConnectionId { get; set; }

        public virtual IEnumerable<RunnerUser> RunnerUsers { get; set; }
    }
}
