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

            if (expression == null)
            {
                throw new ArgumentException();
            }

            var stringExpression = expression is StepVariable customStepVariable ?
                customStepVariable.Value.ToString() :
                expression.ToString();

            var variableNames = GetVariableNames(stringExpression);

            if (variable.GetValueType() == typeof(string))
            {
                return ParseString(stringExpression, variableNames, workflow);
            }

            var parameterExpressions = GetParameterExpressions(variableNames, workflow);
            var sanitizedExpression = stringExpression.Replace(Percentage, string.Empty);

            var workflowVariables = workflow.GetVariables(variableNames);
            var variableValues = workflowVariables.Select(x => x.Value);

            var lambdaExpresion = DynamicExpressionParser.ParseLambda(parameterExpressions.ToArray(), variable.GetValueType(), sanitizedExpression);
            var expressionValue = lambdaExpresion.Compile().DynamicInvoke(variableValues.ToArray());

            return expressionValue;
        }

        private static string ParseString(string expression, IEnumerable<string> variableNames, Workflow workflow)
        {
            foreach(var variableName in variableNames)
            {
                var variable = workflow.GetVariableByName(variableName);
                expression = expression.Replace($"%{variableName}%", variable.GetValue<string>());
            }

            return expression;
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
                if (workflow.VariableWithNameExists(name))
                {
                    var variable = workflow.GetVariableByName(name);
                    var variableExpression = Expression.Parameter(variable.GetValueType(), variable.Id);
                    variableExpressions.Add(variableExpression);
                }
            }

            return variableExpressions;
        }
    }
}
