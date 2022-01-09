using Automaton.Studio.Attributes;
using System;
using System.Reflection;

namespace Automaton.Studio.Conductor
{
    public class StepTypeDescriptor : IStepTypeDescriprot
    {
        public StepDescriptor Describe(Type automatonActivityType)
        {
            var attribute = automatonActivityType.GetCustomAttribute<StepDescriptionAttribute>(false);

            var name = attribute?.Name ?? automatonActivityType.Name;
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