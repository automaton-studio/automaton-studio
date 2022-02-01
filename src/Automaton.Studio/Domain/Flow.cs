using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Domain
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public IList<Definition> Definitions { get; set; }

        public Flow()
        {
            Name = "Untitled";
            Id = Guid.NewGuid().ToString();
            var defaultDefinition = new Definition();
            StartupDefinitionId = defaultDefinition.Id;
            Definitions = new List<Definition> { defaultDefinition };
        }

        public Definition GetStartupDefinition()
        {
            return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);
        }

        public void RemoveDefinition(string id)
        {
            var definition = Definitions.SingleOrDefault(x => x.Id.Equals(id));
            Definitions.Remove(definition);
        }
    }
}
