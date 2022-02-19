using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Automaton.Studio.Domain
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public ExpandoObject Variables { get; set; }
        public IList<Definition> Definitions { get; set; }

        [JsonIgnore]
        public IDictionary<string, object> VariablesDictionary => Variables;

        public Flow()
        {
            var defaultDefinition = new Definition { Flow = this };

            Name = "Untitled";
            Id = Guid.NewGuid().ToString();
            StartupDefinitionId = defaultDefinition.Id;
            Definitions = new List<Definition> { defaultDefinition };
            Variables = new ExpandoObject();
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
