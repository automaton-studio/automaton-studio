using System.Dynamic;
using System.Linq.Expressions;

namespace Automaton.Core.Models
{
    public class Workflow
    {
        private ExpandoObject variables;

        public string Id { get; set; }

        public string Name { get; set; }

        public string StartupDefinitionId { get; set; }

        public ExpandoObject Variables
        {
            get { return variables; }

            set 
            { 
                variables = value;

                foreach (var variable in variables)
                {
                    var variableExpression = Expression.Parameter(variable.Value.GetType(), variable.Key);
                    VariableExpressions.Add(variableExpression);
                }
            }
        }

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

        public void AddVariable(string key, object value)
        {
            var variableExpression = Expression.Parameter(value.GetType(), key);
            VariableExpressions.Add(variableExpression);

            var variablesDictionary = variables as IDictionary<string, object>;
            variablesDictionary.Add(key, value);
        }

        public bool HasVariables()
        {
            var variablesDictionary = variables as IDictionary<string, object>;

            return variablesDictionary.Count > 0;
        }

        public IEnumerable<object> GetVariableValues()
        {
            var variablesDictionary = (IDictionary<string, object>)variables;

            return variablesDictionary.Values;
        }
    }
}
