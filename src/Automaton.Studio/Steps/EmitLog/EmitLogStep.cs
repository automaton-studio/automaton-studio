using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.EmitLog;

[StepDescription(
    Name = "EmitLog",
    Type = "EmitLog",
    DisplayName = "Emit Log",
    Category = "Scripting",
    Description = "Write text to the predefined log environment",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "message"
)]
public class EmitLogStep : StudioStep
{
    public string Message
    {
        get
        {
            return Inputs.ContainsKey(nameof(Message)) ?
                Inputs[nameof(Message)].ToString() : string.Empty;
        }

        set
        {
            Inputs[nameof(Message)] = value;
        }
    }

    public EmitLogStep()
    {
    }

    public override Type GetDesignerComponent()
    {
        return typeof(EmitLogDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(EmitLogProperties);
    }
}
