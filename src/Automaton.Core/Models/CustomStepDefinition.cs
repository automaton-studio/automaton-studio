namespace Automaton.Core.Models;

public class CustomStepDefinition
{
    public string? Code { get; set; }

    public IList<Variable>? CodeInputVariables { get; set; }

    public IList<Variable>? CodeOutputVariables { get; set; }

}
