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
    private const string VariableValueName = nameof(VariableValue);

    public StepVariable VariableValue
    {
        get => GetInputVariable(VariableValueName) as StepVariable;
        set => SetInputVariable(VariableValueName, value);
    }

    public StepVariable VariableOutput =>
        Outputs.ContainsKey(AddVariableKey) ?
        Outputs[AddVariableKey] : null;

    public AddVariableStep()
    {
        SetInputVariable(VariableValueName, new StepVariable());

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
            Key = AddVariableKey,
            OldName = AddVariableKey,
            Name = $"{AddVariableKey}{Flow.GetNumberOfSteps<AddVariableStep>()}",
            Value = VariableValue
        };

        SetVariable(variable);
    }
}
