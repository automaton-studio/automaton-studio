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
        get => GetStringInputVariable(nameof(Message));
        set => SetInputVariable(nameof(Message), value);
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
