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
        public Definition ActiveDefinition => Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);

        public Flow()
        {
            var defaultDefinition = new Definition
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Untitled"
            };

            Name = "Untitled";
            Id = Guid.NewGuid().ToString();
            StartupDefinitionId = defaultDefinition.Id;
            Definitions = new List<Definition> { defaultDefinition };
        }
    }
}
