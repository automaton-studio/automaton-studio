using System.Collections.Generic;

namespace Automaton.Studio.Models
{
    public class TreeActivityModel
    {
        public TreeActivityModel()
        {
            Type = "Activity";
            Category = "Miscellaneous";
            DisplayName = "Activity";
        }

        public TreeActivityModel(string type, string displayName)
        {
            Type = type;
            DisplayName = displayName;
        }

        public TreeActivityModel(string type, string displayName, string? icon, string? category = null)
        {
            Type = type;
            DisplayName = displayName;
            Icon = icon;
            Category = category;
        }

        #region Elsa properties

        public string? Type { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }

        #endregion

        public string? Icon { get; set; }
        public List<TreeActivityModel>? Activities { get; set; }
    }
}
