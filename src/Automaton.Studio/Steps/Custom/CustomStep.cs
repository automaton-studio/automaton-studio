using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.Custom;

[StepDescription(
    Name = "Custom",
    Type = "Custom",
    DisplayName = "Execute Custom Step",
    Category = "Scripting",
    Description = "Executes Custom step",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "code"
)]
public class CustomStep : StudioStep
{
    private const string CodeOutputVariablesName = nameof(CodeOutputVariables);
    private const string CodeInputVariablesName = nameof(CodeInputVariables);

    public string Code
    {
        get => GetStringInputVariable(nameof(Code));
        set => SetInputVariable(nameof(Code), value);
    }

    public IList<StepVariable> CodeInputVariables
    {
        get
        {
            var variables = GetInputVariable(CodeInputVariablesName);
            return variables as IList<StepVariable>;
        }

        set => SetInputVariable(CodeInputVariablesName, value);
    }

    /// <summary>
    /// OutputVariables are stored in Inputs list because they are input required for step execution.
    /// </summary>
    public IList<StepVariable> CodeOutputVariables
    {
        get
        {
            var variables = GetInputVariable(CodeOutputVariablesName);
            return variables as IList<StepVariable>;
        }
        set => SetInputVariable(CodeOutputVariablesName, value);
    }

    public CustomStep()
    {
        SetInputVariable(CodeOutputVariablesName, new List<StepVariable>());
        SetInputVariable(CodeInputVariablesName, new List<StepVariable>());
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
