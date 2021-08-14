using System;

namespace Automaton.Studio.Entities
{
    public class RunnerUser
    {
        public string UserId { get; set; }
        public Guid RunnerId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual Runner Runner { get; set; }
    }
}
