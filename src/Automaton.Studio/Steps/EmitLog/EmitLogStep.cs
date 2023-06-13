using Automaton.Studio.Attributes;
using Automaton.Studio.Domain;

namespace Automaton.Studio.Steps.EmitLog;

[StepDescription(
    Name = "EmitLog",
    Type = "EmitLog",
    DisplayName = "Log message",
    Category = "Messages",
    Description = "Log a flow message",
    MoreInfo = "https://www.automaton.studio/documentation",
    Icon = "message"
)]
public class EmitLogStep : StudioStep
{
    public string Message
    {
        get => GetInputValue(nameof(Message)) as string;
        set => SetInputValue(nameof(Message), value);
    }

    public EmitLogStep()
    {
        SetInputValue(nameof(Message), string.Empty);
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
