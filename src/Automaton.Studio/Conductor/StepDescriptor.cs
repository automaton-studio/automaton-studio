using Automaton.Studio.Conductor.Interfaces;

namespace Automaton.Studio.Conductor
{
    public class StepDescriptor : IStepDescriptor
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; }

        public StepDescriptor()
        {
            Name = "Step";
            Type = "StepType";
            DisplayName = "StepDisplayName";
            Description = "StepDescription";
            Category = "StepCategory";
            Icon = "StepIcon";
        }
    }
}