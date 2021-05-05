using System.Collections.Generic;

namespace Automaton.Studio.Models
{
    public class ActivityTreeModel
    {
        public ActivityTreeModel()
        {
            Type = "Activity";
            Category = "Miscellaneous";
            DisplayName = "Activity";
        }

        #region Elsa properties

        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string? Description { get; set; }
        public string Category { get; set; }

        #endregion

        public string? Icon { get; set; }
        public List<ActivityTreeModel>? Activities { get; set; }
    }
}
