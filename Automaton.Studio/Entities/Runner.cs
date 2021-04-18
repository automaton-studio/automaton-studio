using Automaton.Studio.Entities;
using System;

#nullable disable

namespace Automaton.Studio
{
    public class Runner
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string ConnectionId { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}
