using System;
using System.Collections.Generic;

namespace Automaton.Studio.Conductor
{
    public class Flow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<Definition> Definitions { get; set; } = new List<Definition>();
        public Definition ActiveDefinition { get; set; }
        public string StartupDefinitionId { get; set; }

        public Flow()
        {
            Name = "Untitled";

            var defaultDefinition = new Definition
            {
                Name = "Untitled"
            };

            Definitions.Add(defaultDefinition);
            ActiveDefinition = defaultDefinition;
        }
    }
}
