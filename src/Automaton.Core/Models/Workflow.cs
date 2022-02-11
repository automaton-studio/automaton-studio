namespace Automaton.Core.Models
{
    public class Workflow
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string StartupDefinitionId { get; set; }

        public List<WorkflowDefinition> Definitions { get; set; } = new List<WorkflowDefinition>();

        public WorkflowDefinition GetStartupDefinition()
        {
            return Definitions.SingleOrDefault(x => x.Id == StartupDefinitionId);    
        }
    }
}
