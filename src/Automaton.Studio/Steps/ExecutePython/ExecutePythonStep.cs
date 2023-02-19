using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.ExecutePython;

[StepDescription(
    Name = "ExecutePython",
    Type = "ExecutePython",
    DisplayName = "Execute Python",
    Category = "Scripting",
    Description = "Executes Python script and returns its output",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "code"
)]
public class ExecutePythonStep : StudioStep
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

    public ExecutePythonStep()
    {
        SetInputVariable(nameof(Code), string.Empty);
        SetInputVariable(nameof(CodeOutputVariables), new List<StepVariable>());
        SetInputVariable(nameof(CodeInputVariables), new List<StepVariable>());
    }

    public override Type GetDesignerComponent()
    {
        return typeof(ExecutePythonDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(ExecutePythonProperties);
    }
}
