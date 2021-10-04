using System.Collections.Generic;

namespace Automaton
{
    public class WorkflowDefinition : Entity
    {
        public WorkflowDefinition()
        {
            Variables = new Variables();
            Activities = new List<ActivityDefinition>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }
        public string Data { get; set; }

        public Variables Variables { get; set; }
        public ICollection<ActivityDefinition> Activities { get; set; }
    }
}
