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
        set => Inputs[nameof(Code)] = new StepVariable { Value = value };
    }

    public IList<StepVariable> CodeInputVariables
    {
        get
        {
            if (Inputs.ContainsKey(nameof(CodeInputVariables)))
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
            if (Inputs.ContainsKey(nameof(CodeOutputVariables)))
            {
                if (Inputs[nameof(CodeOutputVariables)].Value is JArray array)
                {
                    Inputs[nameof(CodeOutputVariables)].Value = array.ToObject<List<StepVariable>>();
                }
            }
            else
            {
                SetInputVariable(nameof(CodeOutputVariables), new List<StepVariable>());
            }

            return Inputs[nameof(CodeOutputVariables)].Value as IList<StepVariable>;
        }

        set => SetInputVariable(nameof(CodeOutputVariables), value);
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
