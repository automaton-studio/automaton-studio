using System.Dynamic;

namespace Automaton.Studio.Server.Models
{
    public class Flow
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StartupDefinitionId { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public ExpandoObject Variables { get; set; }
        public List<Definition> Definitions { get; set; } = new List<Definition>();
    }
}
