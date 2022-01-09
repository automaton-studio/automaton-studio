namespace Automaton.Studio.Conductor
{
    public class StepDescriptor : IStepDescriptor
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }

        public StepDescriptor()
        {
            Name = "Activity";
            DisplayName = "Activity";
            Category = "Category";
            Icon = "Icon";
        }
    }
}