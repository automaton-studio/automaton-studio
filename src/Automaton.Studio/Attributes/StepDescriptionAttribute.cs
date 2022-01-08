using System;

namespace Automaton.Studio.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class StepDescriptionAttribute : Attribute
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }
}