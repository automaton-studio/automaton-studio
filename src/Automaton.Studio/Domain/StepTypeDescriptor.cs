using Automaton.Studio.Attributes;
using Automaton.Studio.Domain.Interfaces;
using System.Reflection;

namespace Automaton.Studio.Domain;

public class StepTypeDescriptor : IStepTypeDescriptor
{
    public IStepDescriptor Describe(Type stepType)
    {
        var attribute = stepType.GetCustomAttribute<StepDescriptionAttribute>(false);

        var name = attribute?.Name ?? stepType.Name;
        var type = attribute?.Type;
        var displayName = attribute?.DisplayName;
        var notVisibleInExplorer = attribute?.VisibleInExplorer;
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
            Icon = icon,
            VisibleInExplorer = notVisibleInExplorer.Value
        };
    }
}
