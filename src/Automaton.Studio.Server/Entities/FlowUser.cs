#nullable disable

namespace Automaton.Studio.Server.Entities
{
    public class FlowUser
    {
        public string UserId { get; set; }
        public Guid FlowId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual Flow Flow { get; set; }
    }
}
