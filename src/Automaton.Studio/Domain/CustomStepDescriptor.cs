namespace Automaton.Studio.Domain;

public class CustomStepDescriptor : StepDescriptor
{
    public CustomStepDefinition Definition { get; set; } = new CustomStepDefinition();
}
