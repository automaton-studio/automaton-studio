using System;
using System.Collections.Generic;

namespace Automaton.Studio.Conductor
{
    public class StudioFlow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartupWorkflowId { get; set; }
        public IList<Definition> Workflows { get; set; } = new List<Definition>();
        public Definition ActiveWorkflow { get; set; }

        public StudioFlow()
        {
            Name = "Untitled";

            var defaultDefinition = new Definition
            {
                Name = "Untitled"
            };

            Workflows.Add(defaultDefinition);
            ActiveWorkflow = defaultDefinition;
        }
    }
}
