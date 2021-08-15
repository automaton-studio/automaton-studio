namespace Automaton.Studio.Models
{
    public class TreeWorkflow
    {
        private const string DefaultIcon = "file";

        public string Name { get; set; }
        public bool IsStartup { get; set; }
        public string Icon { get; set; } = DefaultIcon;

        public TreeWorkflow()
        {
            Name = "Workflow";
        }

        public TreeWorkflow(string name)
        {
            Name = name;
        }
    }
}
