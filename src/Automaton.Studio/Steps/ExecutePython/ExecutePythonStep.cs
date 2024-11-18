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
        get => GetInputValue<string>(nameof(Code));
        set => SetInputValue(nameof(Code), value);
    }

    public IList<PythonStepVariable> CodeInputVariables
    {
        get
        {
            var value = GetInputValue<IList<PythonStepVariable>>(nameof(CodeInputVariables));

            return value;
        }

        set => SetInputValue(nameof(CodeInputVariables), value);
    }

    public IList<StepVariable> CodeOutputVariables
    {
        get
        {
            var value = GetInputValue<IList<StepVariable>>(nameof(CodeOutputVariables));

            return value;
        }

        set => SetInputValue(nameof(CodeOutputVariables), value);
    }

    public ExecutePythonStep()
    {
        SetInputValue(nameof(Code), string.Empty);
        SetInputValue(nameof(CodeOutputVariables), new List<StepVariable>());
        SetInputValue(nameof(CodeInputVariables), new List<PythonStepVariable>());
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
