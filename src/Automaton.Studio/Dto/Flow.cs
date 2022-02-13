using System.Collections.Generic;

namespace Automaton.Studio.Dto
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public List<Definition> Definitions { get; set; } = new List<Definition>();
    }
}
