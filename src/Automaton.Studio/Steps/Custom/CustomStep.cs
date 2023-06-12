using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Steps.AddVariable;
using Newtonsoft.Json.Linq;

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

        Created += OnCreated;
    }

    private void OnCreated(object sender, StepEventArgs e)
    {
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
