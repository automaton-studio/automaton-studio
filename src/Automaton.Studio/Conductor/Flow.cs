using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Automaton.Studio.Conductor
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public IList<Definition> Definitions { get; set; } = new List<Definition>();

        [JsonIgnore]
        public Definition ActiveDefinition { get; set; }

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
