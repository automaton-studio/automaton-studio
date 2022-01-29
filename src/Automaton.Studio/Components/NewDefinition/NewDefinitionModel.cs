using System.Collections.Generic;

namespace Automaton.Studio.Components.NewDefinition
{
    public class NewDefinitionModel
    {
        public string Name { get; set; }
        public IEnumerable<string> ExistingNames { get; set; }
    }
}
