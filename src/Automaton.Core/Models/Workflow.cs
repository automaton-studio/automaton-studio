using System.Dynamic;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace Automaton.Core.Models
{
    public class Workflow
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StartupDefinitionId { get; set; }

        public ExpandoObject Variables { get; set; }

        [JsonIgnore]
        public IDictionary<string, object> VariablesDictionary => Variables;

        [JsonIgnore]
        public List<ParameterExpression> VariableExpressions { get; set; }

        public List<WorkflowDefinition> Definitions { get; set; }

        public Workflow()
        {
            Variables = new ExpandoObject();
            VariableExpressions = new List<ParameterExpression>();
            Definitions = new List<WorkflowDefinition>();
        }

        public WorkflowDefinition GetStartupDefinition()
        {
            return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);    
        }

        public void SetVariables(ExpandoObject variables)
        {
            Variables = variables;

            foreach(var variable in Variables)
            {
                var variableExpression = Expression.Parameter(variable.Value.GetType(), variable.Key);
                VariableExpressions.Add(variableExpression);
            }
        }

        public void AddVariable(string key, object value)
        {
            var variableExpression = Expression.Parameter(value.GetType(), key);
            VariableExpressions.Add(variableExpression);
            VariablesDictionary.Add(key, value);
        }

        public bool HasVariables()
        {
            return VariablesDictionary.Count > 0;
        }

        public IEnumerable<object> GetVariableValues()
        {
            return VariablesDictionary.Values;
        }
    }
}
