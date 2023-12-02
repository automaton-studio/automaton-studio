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
        get => GetInputValue<string>(nameof(VariableValue));
        set => SetInputValue(nameof(VariableValue), value);
    }

    public StepVariable OutputVariable { get; set; }

    public AddVariableStep()
    {
        SetInputValue(nameof(VariableValue), string.Empty);

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

    public override void Created()
    {
        base.Created();

        OutputVariable = new StepVariable
        {
            Name = $"{AddVariableKey}{Flow.GetNumberOfSteps<AddVariableStep>()}",
            Value = VariableValue
        };

        SetOutputVariable(OutputVariable);
    }
}
