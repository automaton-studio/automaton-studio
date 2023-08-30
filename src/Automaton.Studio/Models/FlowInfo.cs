namespace Automaton.Studio.Models;

public class FlowInfo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public DateTime Started { get; set; }
    public DateTime Finished { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}
