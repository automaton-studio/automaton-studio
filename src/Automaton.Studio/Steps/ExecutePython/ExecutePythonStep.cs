using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Newtonsoft.Json.Linq;

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
        get => GetInputValue(nameof(Code)) as string;
        set => SetInputValue(nameof(Code), value);
    }

    public IList<StepVariable> CodeInputVariables
    {
        get
        {
            var value = GetInputValue(nameof(CodeInputVariables));

            if (value is JArray)
            {
                return (value as JArray).ToObject<List<StepVariable>>();
            }

            return value as IList<StepVariable>;
        }

        set => SetInputValue(nameof(CodeInputVariables), value);
    }

    public IList<StepVariable> CodeOutputVariables
    {
        get
        {
            var value = GetInputValue(nameof(CodeOutputVariables));

            if (value is JArray)
            {
                return (value as JArray).ToObject<List<StepVariable>>();
            }

            return value as IList<StepVariable>;
        }

        set => SetInputValue(nameof(CodeOutputVariables), value);
    }

    public ExecutePythonStep()
    {
        SetInputValue(nameof(Code), string.Empty);
        SetInputValue(nameof(CodeOutputVariables), new List<StepVariable>());
        SetInputValue(nameof(CodeInputVariables), new List<StepVariable>());
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
