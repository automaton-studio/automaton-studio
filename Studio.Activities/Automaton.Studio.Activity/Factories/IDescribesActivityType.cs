using Automaton.Studio.Activity.Metadata;
using System;

namespace Automaton.Studio.Activities.Factories
{
    public interface IDescribesActivityType
    {
        ActivityDescriptor? Describe(Type activityType);
    }
}