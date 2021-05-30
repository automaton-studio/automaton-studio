namespace Automaton.Studio.Activity.Metadata
{
    public class ActivityDescriptor
    {
        public ActivityDescriptor()
        {
            Name = "Activity";
            ElsaName = "ElsaActivity";
            DisplayName = "Activity";
            Category = "Miscellaneous";
            Category = "Category";
            Icon = "Icon";
            Outcomes = new string[0];
        }

        public string Name { get; set; }
        public string ElsaName { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Icon { get; set; }
        public string[] Outcomes { get; set; }
    }
}