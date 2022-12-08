using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.AddVariable;

[StepDescription(
    Name = "AddVariable",
    Type = "AddVariable",
    DisplayName = "Add Variable",
    Category = "Variables",
    Description = "Add Flow variable",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "field-string"
)]
public class AddVariableStep : StudioStep
{
    public string VariableName
    {
        get => Inputs.ContainsKey(nameof(VariableName)) ?
               Inputs[nameof(VariableName)]?.ToString() : string.Empty;
        set => Inputs[nameof(VariableName)] = value;
    }

    public string OldVariableName { get; set; }

    public string VariableValue
    {
        get => Inputs.ContainsKey(nameof(VariableValue)) ?
               Inputs[nameof(VariableValue)]?.ToString() : string.Empty;
        set => Inputs[nameof(VariableValue)] = value;
    }

    public bool VariableNameIsTheSame()
    {
        return string.Compare(OldVariableName, VariableName, true) == 0;
    }

    public override Type GetDesignerComponent()
    {
        return typeof(AddVariableDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(AddVariableProperties);
    }
}
