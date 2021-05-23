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

        public ActivityDescriptor? Describe(Type automatonActivityType)
        {
            var attribute = automatonActivityType.GetCustomAttribute<ActivityAttribute>(false);

            var name = attribute?.Name ?? automatonActivityType.Name;
            var elsaName = attribute?.ElsaName;
            var displayName = attribute?.DisplayName;
            var description = attribute?.Description;
            var category = attribute?.Category ?? "Miscellaneous";
            var icon = attribute?.Icon;
            var outcomes = attribute?.Outcomes ?? new[] { OutcomeNames.Done };

            return new ActivityDescriptor
            {
                Name = name.Pascalize(),
                ElsaName = elsaName.Pascalize(),
                DisplayName = displayName,
                Description = description,
                Category = category,
                Icon = icon,
                Outcomes = outcomes
            };
        }
    }
}