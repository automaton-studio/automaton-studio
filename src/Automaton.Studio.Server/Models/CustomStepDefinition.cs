using Automaton.Core.Models;

namespace Automaton.Studio.Server.Models;

public class CustomStepDefinition
{
    public string? Code { get; set; }

    public IList<StepVariable>? CodeInputVariables { get; set; }

    public IList<StepVariable>? CodeOutputVariables { get; set; }
}
