using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Automaton.Studio.Conductor
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }

        [JsonIgnore]
        public Definition ActiveDefinition { get; set; }
        public string StartupDefinitionId { get; set; }

        public IList<Definition> Definitions { get; set; } = new List<Definition>();

        public Flow()
        {
            var defaultDefinition = new Definition
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Untitled"
            };
            Definitions.Add(defaultDefinition);

            Name = "Untitled";
            Id = Guid.NewGuid().ToString();
            ActiveDefinition = defaultDefinition;
            StartupDefinitionId = defaultDefinition.Id;
        }
    }
}
