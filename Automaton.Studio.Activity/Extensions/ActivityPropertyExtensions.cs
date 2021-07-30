using Elsa.Models;

namespace Automaton.Studio.Core.Extensions
{
    public static class ActivityPropertyExtensions
    {
        public static string GetExpression(this ActivityDefinitionProperty property)
        {
            return property != null ? property.Expressions[property.Syntax] : string.Empty;
        }

        public static void SetExpression(this ActivityDefinitionProperty property, string expression)
        {
            if (property != null)
            {
                property.Expressions[property.Syntax] = expression;
            }
        }
    }
}
