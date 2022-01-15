using Automaton.Studio.Attributes;
using Automaton.Studio.Conductor.Interfaces;
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
            var type = attribute?.Type;
            var displayName = attribute?.DisplayName;
            var description = attribute?.Description;
            var category = attribute?.Category ?? "Miscellaneous";
            var icon = attribute?.Icon;

            return new StepDescriptor
            {
                Name = name,
                Type = type,
                DisplayName = displayName,
                Description = description,
                Category = category,
                Icon = icon
            };
        }
    }
}