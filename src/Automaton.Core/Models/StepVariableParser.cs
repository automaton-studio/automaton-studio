using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Automaton.Core.Models
{
    public class StepVariableParser
    {
        private const string Percentage = "%";
        private const string VariablePattern = "%.+?%";

        public static object? Parse(StepVariable variable, Workflow workflow)
        {
            var expression = variable.Value;

            var stringExpression = string.Empty;

            if (expression is StepVariable customStepVariable)
            {
                stringExpression = customStepVariable.Value.ToString();
            }
            else
            {
                stringExpression = expression.ToString();
            }

            if (string.IsNullOrEmpty(stringExpression))
            {
                return null;
            }

            var variableNames = GetVariableNames(stringExpression);
            var parameterExpressions = GetParameterExpressions(variableNames, workflow);
            var sanitizedExpression = stringExpression.Replace(Percentage, string.Empty);

            var workflowVariables = workflow.GetVariables(variableNames);
            var variableValues = workflowVariables.Select(x => x.Value);

            var lambdaExpresion = DynamicExpressionParser.ParseLambda(parameterExpressions.ToArray(), variable.GetRealType(), sanitizedExpression);
            var expressionValue = lambdaExpresion.Compile().DynamicInvoke(variableValues.ToArray());

            return expressionValue;
        }

        private static IEnumerable<string> GetVariableNames(string inputString)
        {
            var regex = new Regex(VariablePattern, RegexOptions.IgnoreCase);
            var matches = regex.Matches(inputString);
            var variableNames = matches.Select(x => x.Value.Replace(Percentage, string.Empty));

            return variableNames;
        }

        private static IEnumerable<ParameterExpression> GetParameterExpressions(IEnumerable<string> variableNames, Workflow workflow)
        {
            var variableExpressions = new List<ParameterExpression>();

            foreach (var name in variableNames)
            {
                if (workflow.VariableExists(name))
                {
                    var variable = workflow.GetVariable(name);
                    var variableExpression = Expression.Parameter(variable.GetRealType(), variable.Id);
                    variableExpressions.Add(variableExpression);
                }
            }

            return variableExpressions;
        }
    }
}
