using System;
using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Models
{
    public class StepExplorerModel
    {
        private const string DefaultIcon = "file";

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; } = DefaultIcon;
        public string Category { get; set; }

        public List<StepExplorerModel> Activities { get; set; } = new List<StepExplorerModel>();

        public StepExplorerModel()
        {
            Name = "Step";
            Type = "Step";
        }

        public StepExplorerModel(string name, string displayName)
        {
            Name = name;
            Type = displayName;
        }

        public void AddStep(StepExplorerModel step)
        {
            Activities.Add(step);  
        }

        public bool IsCategory()
        {
            return Activities != null && Activities.Any();
        }
    }
}
