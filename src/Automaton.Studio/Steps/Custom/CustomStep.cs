using Automaton.Core.Models;
using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;
using Newtonsoft.Json.Linq;

namespace Automaton.Studio.Steps.Custom;

[StepDescription(
    Name = "ExecutePython",
    Type = "ExecutePython",
    DisplayName = "Execute Python",
    Category = "Scripting",
    Description = "Executes Python script and returns its output",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "code"
)]
public class CustomStep : StudioStep
{
    public string Code
    {
        get => Inputs.ContainsKey(nameof(Code)) ?
               Inputs[nameof(Code)]?.ToString() : string.Empty;
        set => Inputs[nameof(Code)] = value;
    }

    public IList<Variable> CodeInputVariables
    {
        get
        {
            if (Inputs.ContainsKey(nameof(CodeInputVariables)))
            {
                if (Inputs[nameof(CodeInputVariables)] is JArray array)
                {
                    Inputs[nameof(CodeInputVariables)] = array.ToObject<List<Variable>>();
                }
            }
            else
            {
                Inputs[nameof(CodeInputVariables)] = new List<Variable>();
            }

            return Inputs[nameof(CodeInputVariables)] as IList<Variable>;
        }
        set => Inputs[nameof(CodeInputVariables)] = value;
    }

    /// <summary>
    /// OutputVariables are stored in Inputs list because they are input required for step execution.
    /// </summary>
    public IList<Variable> CodeOutputVariables
    {
        get
        {
            if (Inputs.ContainsKey(nameof(CodeOutputVariables)))
            {
                if (Inputs[nameof(CodeOutputVariables)] is JArray array)
                {
                    Inputs[nameof(CodeOutputVariables)] = array.ToObject<List<Variable>>();
                }
            }
            else
            {
                Inputs[nameof(CodeOutputVariables)] = new List<Variable>();
            }

            return Inputs[nameof(CodeOutputVariables)] as IList<Variable>;
        }
        set => Inputs[nameof(CodeOutputVariables)] = value;
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
