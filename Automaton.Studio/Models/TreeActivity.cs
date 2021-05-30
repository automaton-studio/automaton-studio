using System.Collections.Generic;

namespace Automaton.Studio.Models
{
    public class TreeActivity
    {
        private const string DefaultIcon = "file";

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

        #region Elsa properties

        public string? Name { get; set; }
        public string? DisplayName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }

        #endregion

        public string? Icon { get; set; } = DefaultIcon;
        public List<TreeActivity>? Activities { get; set; }
    }
}
