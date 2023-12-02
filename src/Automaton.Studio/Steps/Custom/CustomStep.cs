using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Custom;

[StepDescription(
    Name = "CustomStep",
    Type = "CustomStep",
    DisplayName = "Execute Custom Step",
    Category = "Custom steps",
    Description = "Executes Custom step",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "code",
    VisibleInExplorer = false
)]
public class CustomStep : StudioStep
{
    public string Code
    {
        get => GetInputValue<string>(nameof(Code));
        set => SetInputValue(nameof(Code), value);
    }

    public IList<StepVariable> CodeInputVariables
    {
        get
        {
            var value = GetInputValue<IList<StepVariable>>(nameof(CodeInputVariables));

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

    public CustomStep()
    {
        SetInputValue(nameof(Code), string.Empty);
        SetInputValue(nameof(CodeOutputVariables), new List<StepVariable>());
        SetInputValue(nameof(CodeInputVariables), new List<StepVariable>());
    }

    public override void Created()
    {
        base.Created();

        foreach (var codeOutputVariable in CodeOutputVariables)
        {
            var outputVariable = new StepVariable
            {
                Id = codeOutputVariable.Id,
                Name = Flow.GenerateVariableName<CustomStep>(codeOutputVariable.Name),
                Description = codeOutputVariable.Description
            };

            SetOutputVariable(outputVariable);
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
