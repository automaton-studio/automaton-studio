using System.Collections.Generic;

namespace Automaton.Studio.Models
{
    public class WorkflowModel
    {
        private const string DefaultIcon = "file";

        public string Name { get; set; } = "Workflow";
        public bool IsStartup { get; set; }
        public string Icon { get; set; } = DefaultIcon;
        public IList<WorkflowModel> Workflows { get; set; }
    }
}
