using System;

namespace Automaton.Studio.Activities.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ActivityAttribute : Attribute
    {
        public string? Name { get; set; }
        public string? ElsaName { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public string? Icon { get; set; }
        public string[]? Outcomes { get; set; }
    }
}