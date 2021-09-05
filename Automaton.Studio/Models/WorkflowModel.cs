namespace Automaton.Studio.Models
{
    public class WorkflowModel
    {
        #region Constants

        private const string DefaultIcon = "file";
        private const string DefaultWorkflow = "Workflow";

        #endregion

        #region Properties

        public string Id { get; set; }
        public string Name { get; set; } = DefaultWorkflow;
        public bool IsStartup { get; set; }
        public string Icon { get; set; } = DefaultIcon;

        #endregion
    }
}
