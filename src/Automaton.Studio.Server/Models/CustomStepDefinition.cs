using Automaton.Core.Models;

namespace Automaton.Studio.Server.Models;

public class CustomStepDefinition
{
    public string? Code { get; set; }

    public IList<CustomStepVariable>? CodeInputVariables { get; set; } = new List<CustomStepVariable>();

    public IList<CustomStepVariable>? CodeOutputVariables { get; set; } = new List<CustomStepVariable>();
}
