namespace Automaton.Studio.Activity.Metadata
{
    public class ActivityDescriptor
    {
        public ActivityDescriptor()
        {
            Type = "Activity";
            Category = "Miscellaneous";
            DisplayName = "Activity";
            Category = "Category";
            Icon = "Icon";
            Outcomes = new string[0];
        }

        public string Type { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Icon { get; set; }
        public string[] Outcomes { get; set; }
    }
}