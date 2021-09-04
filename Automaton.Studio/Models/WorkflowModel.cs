namespace Automaton.Studio.Models
{
    public class WorkflowModel
    {
        #region Constants

        public const string DefaultIcon = "file";

        #endregion

        #region Properties

        public string Id { get; set; }
        public string Name { get; set; } = "Workflow";
        public bool IsStartup { get; set; }
        public string Icon { get; set; } = DefaultIcon;

        #endregion
    }
}
