using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Models
{
    public class SolutionStep
    {
        private const string DefaultIcon = "file";

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; } = DefaultIcon;
        public string Category { get; set; }

        public List<SolutionStep> Activities { get; set; } = new List<SolutionStep>();

        public SolutionStep()
        {
            Name = "Step";
            Type = "Step";
        }

        public SolutionStep(string name, string displayName)
        {
            Name = name;
            Type = displayName;
        }

        public void AddStep(SolutionStep step)
        {
            Activities.Add(step);  
        }

        public bool IsCategory()
        {
            return Activities != null && Activities.Any();
        }
    }
}
