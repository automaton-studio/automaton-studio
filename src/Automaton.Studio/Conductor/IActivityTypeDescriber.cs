using System;

namespace Automaton.Studio.Conductor
{
    public interface IActivityTypeDescriber
    {
        ActivityDescriptor Describe(Type activityType);
    }
}