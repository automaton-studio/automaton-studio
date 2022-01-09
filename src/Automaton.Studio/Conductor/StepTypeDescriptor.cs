using Automaton.Studio.Attributes;
using System;
using System.Reflection;

namespace Automaton.Studio.Conductor
{
    public class StepTypeDescriptor : IStepTypeDescriptor
    {
        public StepDescriptor Describe(Type stepType)
        {
            var attribute = stepType.GetCustomAttribute<StepDescriptionAttribute>(false);

            var name = attribute?.Name ?? stepType.Name;
            var displayName = attribute?.DisplayName;
            var description = attribute?.Description;
            var category = attribute?.Category ?? "Miscellaneous";
            var icon = attribute?.Icon;

            return new StepDescriptor
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