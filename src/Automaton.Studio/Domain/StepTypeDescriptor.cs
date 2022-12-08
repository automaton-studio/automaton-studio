using Automaton.Studio.Attributes;
using Automaton.Studio.Domain.Interfaces;
using System.Reflection;

namespace Automaton.Studio.Domain;

public class StepTypeDescriptor : IStepTypeDescriptor
{
    public IStepDescriptor Describe(Type stepType)
    {
        var attribute = stepType.GetCustomAttribute<StepDescriptionAttribute>(false);

        return new StepDescriptor
        {
            Name = attribute?.Name ?? stepType.Name,
            Type = attribute?.Type,
            DisplayName = attribute?.DisplayName,
            Description = attribute?.Description,
            MoreInfo = attribute?.MoreInfo,
            Category = attribute?.Category ?? "Miscellaneous",
            Icon = attribute?.Icon,
            VisibleInExplorer = attribute.VisibleInExplorer
        };
    }
}
