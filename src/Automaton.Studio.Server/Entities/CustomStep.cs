namespace Automaton.Studio.Server.Entities
{
    public class CustomStep
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string MoreInfo { get; set; }
        public string Icon { get; set; }
        public string Definition { get; set; }
        public string Category { get; set; }
        public bool VisibleInExplorer { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public virtual ICollection<CustomStepUser> CustomStepUsers { get; set; }
    }
}
