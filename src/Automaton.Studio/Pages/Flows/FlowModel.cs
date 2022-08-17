using System;
using System.Collections.Generic;

namespace Automaton.Studio.Pages.Flows;

public class FlowModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public IEnumerable<Guid> RunnerIds = new List<Guid>();
}
