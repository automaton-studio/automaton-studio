using System.Dynamic;
using System.Text.Json.Serialization;

namespace Automaton.Core.Models
{
    public class Workflow
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StartupDefinitionId { get; set; }

        public ExpandoObject Variables { get; set; }

        public List<WorkflowDefinition> Definitions { get; set; }

        [JsonIgnore]
        public IDictionary<string, object> VariablesDictionary => Variables;

        public Workflow()
        {
            Variables = new ExpandoObject();
            Definitions = new List<WorkflowDefinition>();
        }

        public WorkflowDefinition GetStartupDefinition()
        {
            return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);    
        }
    }
}
