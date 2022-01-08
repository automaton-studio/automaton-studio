using Automaton.Studio.Attributes;
using System;
using System.Reflection;

namespace Automaton.Studio.Conductor
{
    public class ActivityTypeDescriber : IActivityTypeDescriber
    {
        public ActivityDescriptor Describe(Type automatonActivityType)
        {
            var attribute = automatonActivityType.GetCustomAttribute<StepDescriptionAttribute>(false);

            var name = attribute?.Name ?? automatonActivityType.Name;
            var displayName = attribute?.DisplayName;
            var description = attribute?.Description;
            var category = attribute?.Category ?? "Miscellaneous";
            var icon = attribute?.Icon;

            return new ActivityDescriptor
            {
                Name = name,
                DisplayName = displayName,
                Description = description,
                Category = category,
                Icon = icon
            };
        }
    }
}