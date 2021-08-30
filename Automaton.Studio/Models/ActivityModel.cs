using System.Collections.Generic;
using System.Linq;

namespace Automaton.Studio.Models
{
    public class ActivityModel
    {
        private const string DefaultIcon = "file";

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Icon { get; set; } = DefaultIcon;
        public List<ActivityModel> Activities { get; set; }

        public ActivityModel()
        {
            Name = "Activity";
            Category = "Miscellaneous";
            DisplayName = "Activity";
        }

        public ActivityModel(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public bool IsCategory()
        {
            return Activities != null && Activities.Any();
        }
    }
}
