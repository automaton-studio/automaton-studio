using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace Automaton.Studio.Domain
{
    public class StudioFlow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public IDictionary<string, object> Variables { get; set; }
        public IDictionary<string, object> OutputVariables { get; set; }
        public IDictionary<string, object> VariablesDictionary => Variables;
        public IDictionary<string, object> OutputVariablesDictionary => OutputVariables;
        public IList<StudioDefinition> Definitions { get; set; }

        public StudioFlow()
        {
            Name = "Untitled";
            var defaultDefinition = new StudioDefinition { Flow = this };
            StartupDefinitionId = defaultDefinition.Id;
            Definitions = new List<StudioDefinition> { defaultDefinition };
            Variables = new Dictionary<string, object>();
            OutputVariables = new Dictionary<string, object>();
        }

        public StudioDefinition GetStartupDefinition()
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
