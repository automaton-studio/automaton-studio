using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

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
    private const string DefaultVariableBaseName = "NewVar";

    public StringStepVariable Variable
    {
        get => GetInputValue<StringStepVariable>(nameof(Variable));
        set => SetInputValue(nameof(Variable), value);
    }

    public AddVariableStep()
    {
        SetInputValue(nameof(Variable), string.Empty);

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

        Variable = new StringStepVariable
        {
            Id = Guid.NewGuid().ToString(),
            Name = $"{DefaultVariableBaseName}{Flow.GetNumberOfSteps<AddVariableStep>()}",
            Value = string.Empty
        };

        SetOutputVariable();
    }

    public void SetOutputVariable()
    {
        SetOutputVariable(Variable);
    }
}
