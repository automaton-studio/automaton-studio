using Automaton.Studio.Components.NewVariable;
using Automaton.Studio.Events;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Automaton.Studio.Domain
{
    public class Flow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public ExpandoObject Variables { get; set; }
        public ExpandoObject OutputVariables { get; set; }
        public IDictionary<string, object> VariablesDictionary => Variables;
        public IDictionary<string, object> OutputVariablesDictionary => OutputVariables;
        public IList<Definition> Definitions { get; set; }

        public Flow()
        {
            var defaultDefinition = new Definition { Flow = this };

            Name = "Untitled";
            StartupDefinitionId = defaultDefinition.Id;
            Definitions = new List<Definition> { defaultDefinition };
            Variables = new ExpandoObject();
            OutputVariables = new ExpandoObject();
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

        public void SetOutputVariable(string key, object value)
        {
            if (OutputVariablesDictionary.ContainsKey(key))
            {
                OutputVariablesDictionary[key] = value;
            }
            else
            {
                OutputVariablesDictionary.Add(key, value);
            }
        }

        public IEnumerable<string> GetVariableNames()
        {
            return VariablesDictionary.Keys;
        }

        public IEnumerable<string> GetOutputVariableNames()
        {
            return OutputVariablesDictionary.Keys;
        }

        public void DeleteVariable(string variable)
        {
            VariablesDictionary.Remove(variable);
        }

        public void DeleteOutputVariable(string variable)
        {
            OutputVariablesDictionary.Remove(variable);
        }

        public void DeleteVariables(IEnumerable<string> variables)
        {
            foreach (var variable in variables)
            {
                VariablesDictionary.Remove(variable);
            }
        }
    }
}
