namespace Automaton.Studio.Conductor
{
    public class ActivityDescriptor
    {
        public ActivityDescriptor()
        {
            Name = "Activity";
            DisplayName = "Activity";
            Category = "Category";
            Icon = "Icon";
        }

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }
    }
}