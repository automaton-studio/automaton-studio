using System;

namespace Automaton.Studio.Activity.Metadata
{
    public interface IActivityTypeDescriber
    {
        ActivityDescriptor? Describe(Type activityType);
    }
}