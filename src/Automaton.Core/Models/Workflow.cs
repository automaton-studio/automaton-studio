using System.Dynamic;

namespace Automaton.Core.Models
{
    public class Workflow
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string StartupDefinitionId { get; set; }

        public ExpandoObject Variables { get; set; }

        public ExpandoObject OutputVariables { get; set; }

        public List<WorkflowDefinition> Definitions { get; set; }

        public Workflow()
        {
            Variables = new ExpandoObject();
            OutputVariables = new ExpandoObject();
            Definitions = new List<WorkflowDefinition>();
        }

        public WorkflowDefinition GetStartupDefinition()
        {
            return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);    
        }

        public KeyValuePair<string, object> GetVariable(string key)
        {
            var dictionary = Variables as IDictionary<string, object>;

            return new KeyValuePair<string, object>(key, dictionary[key]);
        }

        public void AddVariable(string key, object value)
        {
            var variablesDictionary = Variables as IDictionary<string, object>;
            variablesDictionary.Add(key, value);
        }

        public bool HasVariables()
        {
            var variablesDictionary = Variables as IDictionary<string, object>;

            return variablesDictionary.Count > 0;
        }

        public IEnumerable<KeyValuePair<string, object>> GetVariables(IEnumerable<string> names)
        {
            var variablesDictionary = (IDictionary<string, object>)Variables;

            var variables = variablesDictionary.Where(x => names.Contains(x.Key)).Select(x => new KeyValuePair<string, object>(x.Key, x.Value));

            return variables;
        }
    }
}
