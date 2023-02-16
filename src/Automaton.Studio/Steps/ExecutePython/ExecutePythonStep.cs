using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.ExecutePython;

[StepDescription(
    Name = "ExecutePython",
    Type = "ExecutePython",
    DisplayName = "Execute Python",
    Category = "Scripting",
    Description = "Executes Python script and returns its output",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "code"
)]
public class ExecutePythonStep : StudioStep
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

    public ExecutePythonStep()
    {
        SetInputVariable(CodeOutputVariablesName, new List<StepVariable>());
        SetInputVariable(CodeInputVariablesName, new List<StepVariable>());
    }

    public override Type GetDesignerComponent()
    {
        return typeof(ExecutePythonDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(ExecutePythonProperties);
    }
}
