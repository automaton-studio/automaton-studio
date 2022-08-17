namespace Automaton.Studio.Domain.Interfaces;

public interface IStepTypeDescriptor
{
    IStepDescriptor Describe(Type stepType);
}
