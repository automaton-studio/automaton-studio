#nullable disable

namespace Automaton.Studio.Server.Entities
{
    public class Flow
    {
        public Guid Id { get; set; }
        public string Body {  get; set; }

        public virtual ICollection<FlowUser> FlowUsers { get; set; }
    }
}
