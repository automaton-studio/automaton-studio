using Automaton.Studio.Attributes;
using Automaton.Studio.Factories;
using Automaton.Studio.Steps.Sequence;

namespace Automaton.Studio.Steps.If;

[StepDescription(
    Name = "If",
    Type = "If",
    DisplayName = "If",
    Category = "Flow control",
    Description = "If conditional statement",
    Icon = "fork"
)]
public class IfStep : SequenceStep
{
    public string Expression
    {
        get => GetInputValue(nameof(Expression)) as string;
        set => SetInputValue(nameof(Expression), value);
    }

    public string Error { get; set; }

    public IfStep(StepFactory stepFactory) : base(stepFactory)
    {
        SetInputValue(nameof(Expression), string.Empty);
    }

    public override Type GetDesignerComponent()
    {
        return typeof(IfDesigner);
    }

    public override Type GetPropertiesComponent()
    {
        return typeof(IfProperties);
    }
}
