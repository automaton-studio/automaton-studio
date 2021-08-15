using System;

namespace Automaton.Studio.Entities
{
    public class FlowUser
    {
        public string UserId { get; set; }
        public Guid FlowId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual Flow Flow { get; set; }
    }
}
