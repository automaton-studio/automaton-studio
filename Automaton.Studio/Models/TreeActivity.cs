using System.Collections.Generic;

namespace Automaton.Studio.Models
{
    public class TreeActivity
    {
        public TreeActivity()
        {
            Name = "Activity";
            Category = "Miscellaneous";
            DisplayName = "Activity";
        }

        public TreeActivity(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public TreeActivity(string name, string displayName, string? icon, string? category = null)
        {
            Name = name;
            DisplayName = displayName;
            Icon = icon;
            Category = category;
        }

        #region Elsa properties

        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }

        #endregion

        public string? Icon { get; set; }
        public List<TreeActivity>? Activities { get; set; }
    }
}
