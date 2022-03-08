using System.Dynamic;
using System.Linq.Expressions;

namespace Automaton.Core.Models
{
    public class Workflow
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StartupDefinitionId { get; set; }

        public ExpandoObject Variables { get; set; }

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
            var variablesDictionary = Variables as IDictionary<string, object>;
            variablesDictionary.Add(key, value);
        }

        public bool HasVariables()
        {
            var variablesDictionary = Variables as IDictionary<string, object>;

            return variablesDictionary.Count > 0;
        }

        public IEnumerable<object> GetVariableValues()
        {
            var variablesDictionary = (IDictionary<string, object>)Variables;

            return variablesDictionary.Values;
        }
    }
}
