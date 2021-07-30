using System;

namespace Automaton.Studio.Core.Metadata
{
    public interface IActivityTypeDescriber
    {
        ActivityDescriptor Describe(Type activityType);
    }
}