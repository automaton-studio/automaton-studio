using System.Collections.Generic;

namespace Automaton.Studio.Components.Dialogs.WorkflowName
{
    public class WorkflowNameModel
    {
        public string Name { get; set; }
        public IEnumerable<string> ExistingNames { get; set; }
    }
}
