using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;

namespace Automaton.Studio.Steps.AddVariable;

[StepDescription(
    Name = "AddVariable",
    Type = "AddVariable",
    DisplayName = "Add Variable",
    Category = "Variables",
    Description = "Set the value of a new or existing variable",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "field-string"
)]
public class AddVariableStep : StudioStep
{
    private const string AddVariableKey = "NewVar";

    public StepVariable VariableValue
    {
        get => GetInputValue(nameof(VariableValue)) as StepVariable;
        set => SetInputValue(nameof(VariableValue), value);
    }

    public StepVariable VariableOutput => GetOutputValue(AddVariableKey) as StepVariable;

    public AddVariableStep()
    {
        SetInputValue(nameof(VariableValue), new StepVariable());
        SetOutputVariable(new StepVariable(AddVariableKey));

        Created += OnCreated;
        ShowVariables = false;
    }

    public override Type GetDesignerComponent()
    {
        return typeof(AddVariableDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(AddVariableProperties);
    }

    private void OnCreated(object sender, StepEventArgs e)
    {
        var variable = new StepVariable
        {
            Name = $"{AddVariableKey}{Flow.GetNumberOfSteps<AddVariableStep>()}",
            Value = VariableValue
        };

        SetOutputVariable(variable);
    }
}
