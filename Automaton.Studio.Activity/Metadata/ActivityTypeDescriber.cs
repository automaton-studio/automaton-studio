using Elsa;
using Humanizer;
using System;
using System.Reflection;

namespace Automaton.Studio.Activity.Metadata
{
    public class ActivityTypeDescriber : IActivityTypeDescriber
    {
        public ActivityDescriptor Describe(Type automatonActivityType)
        {
            var attribute = automatonActivityType.GetCustomAttribute<StudioActivityAttribute>(false);

            var name = attribute?.Name ?? automatonActivityType.Name;
            var displayName = attribute?.DisplayName;
            var description = attribute?.Description;
            var category = attribute?.Category ?? "Miscellaneous";
            var icon = attribute?.Icon;
            var outcomes = attribute?.Outcomes ?? OutcomeNames.Done;

            return new ActivityDescriptor
            {
                Name = name.Pascalize(),
                DisplayName = displayName,
                Description = description,
                Category = category,
                Icon = icon,
                Outcomes = outcomes
            };
        }
    }
}