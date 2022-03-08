using Automaton.Studio.Components.NewVariable;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Automaton.Studio.Domain
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public ExpandoObject Variables { get; set; }
        public IDictionary<string, object> VariablesDictionary => Variables;
        public IList<Definition> Definitions { get; set; }

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

        public void SetVariable(string key, object value)
        {
            if (VariablesDictionary.ContainsKey(key))
            {
                VariablesDictionary[key] = value;
            }
            else
            {
                VariablesDictionary.Add(key, value);
            }
        }

        public void DeleteVariable(string key)
        {
            VariablesDictionary.Remove(key);
        }

        public IEnumerable<Variable> GetVariables()
        {
            return VariablesDictionary.Select(x => new Variable
            {
                Name = x.Key,
                Value = x.Value.ToString()
            });
        }

        public IEnumerable<string> GetVariableNames()
        {
            return VariablesDictionary.Keys;
        }
    }
}
