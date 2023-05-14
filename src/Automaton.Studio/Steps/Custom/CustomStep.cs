using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Newtonsoft.Json.Linq;

namespace Automaton.Studio.Steps.Custom;

[StepDescription(
    Name = "CustomStep",
    Type = "CustomStep",
    DisplayName = "Execute Custom Step",
    Category = "Scripting",
    Description = "Executes Custom step",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "code",
    VisibleInExplorer = false
)]
public class CustomStep : StudioStep
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

    public CustomStep()
    {
        SetInputValue(nameof(Code), string.Empty);
        SetInputValue(nameof(CodeOutputVariables), new List<StepVariable>());
        SetInputValue(nameof(CodeInputVariables), new List<StepVariable>());
    }

    public override void Setup(Step step)
    {
        base.Setup(step);

        Code = step.Inputs[nameof(Code)].Value as string;

        if (step.Inputs[nameof(CodeInputVariables)].Value is JArray inputVariablesArray)
        {
            var stepProperty = GetType().GetProperty(nameof(CodeInputVariables));
            CodeInputVariables = inputVariablesArray.ToObject(stepProperty.PropertyType) as IList<StepVariable>;
        }

        if (step.Inputs[nameof(CodeOutputVariables)].Value is JArray outputVariablesArray)
        {
            var stepProperty = GetType().GetProperty(nameof(CodeOutputVariables));
            CodeOutputVariables = outputVariablesArray.ToObject(stepProperty.PropertyType) as IList<StepVariable>;
        }
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
