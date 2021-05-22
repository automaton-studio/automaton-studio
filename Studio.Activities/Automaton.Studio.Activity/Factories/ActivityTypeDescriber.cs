using Automaton.Studio.Activities.Attributes;
using Automaton.Studio.Activity.Metadata;
using Elsa;
using Humanizer;
using System;
using System.Reflection;

namespace Automaton.Studio.Activities.Factories
{
    public class ActivityTypeDescriber : IDescribesActivityType
    {

        public ActivityTypeDescriber()
        {
        }

        public ActivityDescriptor? Describe(Type activityType)
        {
            var activityAttribute = activityType.GetCustomAttribute<ActivityAttribute>(false);
            var typeName = activityAttribute?.Type ?? activityType.Name;
            var displayName = activityAttribute?.DisplayName;
            var description = activityAttribute?.Description;
            var category = activityAttribute?.Category ?? "Miscellaneous";
            var icon = activityAttribute?.Icon;
            var outcomes = activityAttribute?.Outcomes ?? new[] { OutcomeNames.Done };

            return new ActivityDescriptor
            {
                Type = typeName.Pascalize(),
                DisplayName = displayName,
                Description = description,
                Category = category,
                Icon = icon,
                Outcomes = outcomes
            };
        }
    }
}