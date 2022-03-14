using System.Collections.Generic;
using System.Dynamic;

namespace Automaton.Studio.Dto
{
    public class Flow
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public ExpandoObject Variables { get; set; }
        public ExpandoObject OutputVariables { get; set; }
        public List<Definition> Definitions { get; set; } = new List<Definition>();
    }
}
