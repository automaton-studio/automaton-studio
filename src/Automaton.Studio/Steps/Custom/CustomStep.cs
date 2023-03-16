using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Custom;

[StepDescription(
    Name = "CustomStep",
    Type = "CustomStep",
    DisplayName = "Execute Custom Step",
    Category = "Scripting",
    Description = "Executes Custom step",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "code"
)]
public class CustomStep : StudioStep
{
    public string Code
    {
        get => GetInputVariable(nameof(Code)) as string;
        set => SetInputVariable(nameof(Code), value);
    }

    public IList<StepVariable> CodeInputVariables
    {
        get => GetInputVariable(nameof(CodeInputVariables)) as IList<StepVariable>;
        set => SetInputVariable(nameof(CodeInputVariables), value);
    }

    /// <summary>
    /// OutputVariables are stored in Inputs list because they are input required for step execution.
    /// </summary>
    public IList<StepVariable> CodeOutputVariables
    {
        get => GetInputVariable(nameof(CodeOutputVariables)) as IList<StepVariable>;
        set => SetInputVariable(nameof(CodeOutputVariables), value);
    }

    public CustomStep()
    {
        SetInputVariable(nameof(Code), string.Empty);
        SetInputVariable(nameof(CodeOutputVariables), new List<StepVariable>());
        SetInputVariable(nameof(CodeInputVariables), new List<StepVariable>());
    }

    public override Type GetDesignerComponent()
    {
        return typeof(CustomDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(CustomProperties);
    }
}
