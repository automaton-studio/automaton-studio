#nullable disable

namespace Automaton.Studio.Server.Models
{
    public class FlowInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Body {  get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
