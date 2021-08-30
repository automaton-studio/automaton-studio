namespace Automaton.Studio.Models
{
    public class WorkflowModel
    {
        private const string DefaultIcon = "file";

        public string Name { get; set; }
        public bool IsStartup { get; set; }
        public string Icon { get; set; } = DefaultIcon;

        public WorkflowModel()
        {
            Name = "Workflow";
        }

        public WorkflowModel(string name)
        {
            Name = name;
        }
    }
}
