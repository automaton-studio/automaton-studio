using System.Collections.Generic;

namespace Automaton
{
    public class Workflow
    {
        public Workflow()
        {
            Variables = new Variables();
            Activities = new List<Activity>();
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool IsPublished { get; set; }

        public Variables Variables { get; set; }
        public ICollection<Activity> Activities { get; set; }

        protected virtual void OnEnter()
        {
        }

        protected virtual void OnExit()
        {
        }
    }
}
