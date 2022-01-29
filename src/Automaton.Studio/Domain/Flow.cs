using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Automaton.Studio.Domain
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public IList<Definition> Definitions { get; set; }

        [JsonIgnore]
        public Definition ActiveDefinition { get; set; }

        public Flow()
        {
            Definitions = new List<Definition>();

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

        public void SetupActiveDefinition()
        {
            ActiveDefinition = Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);
        }
    }
}
