using System.Collections.Generic;

namespace Automaton
{
    public class ActivityDefinition : Entity
    {
        public string Type { get; set; } = default!;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public ICollection<ActivityDefinitionProperty> Properties { get; set; } = new List<ActivityDefinitionProperty>();
    }
}
