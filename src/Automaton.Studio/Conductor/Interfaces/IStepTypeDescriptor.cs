using System;

namespace Automaton.Studio.Conductor.Interfaces
{
    public interface IStepTypeDescriptor
    {
        StepDescriptor Describe(Type stepType);
    }
}