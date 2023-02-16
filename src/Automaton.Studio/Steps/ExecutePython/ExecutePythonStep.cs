using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Newtonsoft.Json.Linq;

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

    public string Code
    {
        get => Inputs.ContainsKey(nameof(Code)) ?
               Inputs[nameof(Code)].Value.ToString() : string.Empty;
        set => SetInputVariable(nameof(Code), value);
    }

    public IList<StepVariable> CodeInputVariables
    {
        get
        {
            if (InputVariableExists(nameof(CodeInputVariables)))
            {
                if (Inputs[nameof(CodeInputVariables)].Value is JArray array)
                {
                    Inputs[nameof(CodeInputVariables)].Value = array.ToObject<List<StepVariable>>();
                }
            }
            else
            {
                SetInputVariable(nameof(CodeInputVariables), new List<StepVariable>());
            }

            return Inputs[nameof(CodeInputVariables)].Value as IList<StepVariable>;
        }

        set => SetInputVariable(nameof(CodeInputVariables), value);
    }

    /// <summary>
    /// OutputVariables are stored in Inputs list because they are input required for step execution.
    /// </summary>
    public IList<StepVariable> CodeOutputVariables
    {
        get
        {
            var variables = GetInputVariable(CodeOutputVariablesName) as JArray;

            return variables.ToObject<List<StepVariable>>();
        }
        set => SetInputVariable(CodeOutputVariablesName, value);
    }

    public ExecutePythonStep()
    {
        SetInputVariable(nameof(CodeOutputVariables), new List<StepVariable>());
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
