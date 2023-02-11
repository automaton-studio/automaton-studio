using Automaton.Core.Models;

namespace Automaton.Studio.Server.Models;

public class CustomStep
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public CustomStepDefinition Definition { get; set; }
}

