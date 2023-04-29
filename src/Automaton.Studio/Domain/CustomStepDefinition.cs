using Automaton.Core.Models;

namespace Automaton.Studio.Domain;

public class CustomStepDefinition
{
    public string Code { get; set; }

    public IList<StepVariable> CodeInputVariables { get; set; } = new List<StepVariable>();

    public IList<StepVariable> CodeOutputVariables { get; set; } = new List<StepVariable>();
}
