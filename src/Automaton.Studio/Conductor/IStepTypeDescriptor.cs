using System;

namespace Automaton.Studio.Conductor
{
    public interface IStepTypeDescriptor
    {
        StepDescriptor Describe(Type stepType);
    }
}