using Automaton.Core.Models;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Automaton.Core.Parsers
{
    public class ExpressionParser
    {
        private const string Percentage = "%";
        private const string VariablePattern = "%.+?%";

        public static object Parse(object expression, Workflow workflow)
        {
            var stringExpression = string.Empty;

            if (expression is StepVariable customStepVariable)
            {
                stringExpression = customStepVariable.Value.ToString();
            }
            else
            {
                stringExpression = expression.ToString();
            }

            var variableNames = GetVariableNames(stringExpression);
            var parameterExpressions = GetParameterExpressions(variableNames, workflow);
            var sanitizedExpression = expression.ToString().Replace(Percentage, string.Empty);

            if (string.IsNullOrEmpty(sanitizedExpression))
                return sanitizedExpression;

            var workflowVariables = workflow.GetVariables(variableNames);
            var variableValues = workflowVariables.Select(x => x.Value?.Value);

            var lambdaExpresion = DynamicExpressionParser.ParseLambda(parameterExpressions.ToArray(), null, sanitizedExpression);
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
                var variable = workflow.GetVariable(name);
                var variableExpression = Expression.Parameter(typeof(string), variable.Key);
                variableExpressions.Add(variableExpression);
            }

            return variableExpressions;
        }
    }
}
