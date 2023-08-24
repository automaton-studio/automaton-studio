#nullable disable

namespace Automaton.Studio.Server.Entities
{
    public class FlowExecution
    {
        public Guid Id { get; set; }
        public Guid FlowId { get; set; }
        public DateTime Started { get; set; }
        public DateTime Finished { get; set; }
        public string Status { get; set; }

        public virtual ICollection<FlowExecutionUser> FlowExecutionUsers { get; set; }
    }
}
