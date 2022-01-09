using System;

namespace Automaton.Studio.Conductor
{
    public interface IStepTypeDescriprot
    {
        StepDescriptor Describe(Type activityType);
    }
}