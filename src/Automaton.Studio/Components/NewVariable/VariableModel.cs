using System.Collections.Generic;

namespace Automaton.Studio.Components.NewVariable
{
    public class VariableModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public IEnumerable<string> ExistingNames { get; set; }
    }
}
