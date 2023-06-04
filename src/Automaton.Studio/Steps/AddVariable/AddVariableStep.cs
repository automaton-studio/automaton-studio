using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using IronPython.Compiler.Ast;

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
        get => GetInputValue(nameof(VariableValue)) as string;
        set => SetInputValue(nameof(VariableValue), value);
    }

    public StepVariable VariableOutput { get; set; }

    public AddVariableStep()
    {
        SetInputValue(nameof(VariableValue), string.Empty);

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
        VariableOutput = new StepVariable
        {
            Name = $"{AddVariableKey}{Flow.GetNumberOfSteps<AddVariableStep>()}",
            Value = VariableValue
        };

        SetOutputVariable(VariableOutput);
    }
}
