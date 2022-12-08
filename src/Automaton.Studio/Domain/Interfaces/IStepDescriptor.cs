namespace Automaton.Studio.Domain.Interfaces
{
    public interface IStepDescriptor
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string MoreInfo { get; set; }
        public string Category { get; set; }
        public bool VisibleInExplorer { get; set; }
        public string Icon { get; set; }
    }
}