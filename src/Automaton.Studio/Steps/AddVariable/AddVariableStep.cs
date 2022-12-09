using AntDesign;
using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Resources;
using Automaton.Studio.Steps.TestReport;

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

    public string VariableValue
    {
        get => Inputs.ContainsKey(nameof(VariableValue)) ?
               Inputs[nameof(VariableValue)]?.ToString() : string.Empty;
        set => Inputs[nameof(VariableValue)] = value;
    }

    public StepVariable VariableOutput =>
        Outputs.ContainsKey(AddVariableKey) ?
        Outputs[AddVariableKey] : null;

    public AddVariableStep()
    {
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
