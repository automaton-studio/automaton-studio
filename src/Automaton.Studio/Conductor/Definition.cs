using System;
using System.Collections.Generic;

namespace Automaton.Studio.Conductor
{
    public class Definition
    {
        public string Id { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

        public List<Step> Steps { get; set; } = new List<Step>();
        
    }
}
