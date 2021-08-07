using Automaton.Studio.Entities;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Entities
{
    public class Flow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual IEnumerable<FlowWorkflow> FlowWorkflows { get; set; }
    }
}
