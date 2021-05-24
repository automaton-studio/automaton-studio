using System;

namespace Automaton.Studio.Metadata
{
    public interface IActivityTypeDescriber
    {
        ActivityDescriptor? Describe(Type activityType);
    }
}