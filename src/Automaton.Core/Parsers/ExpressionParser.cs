﻿using Automaton.Core.Models;
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
            var variableNames = GetVariableNames(expression);
            var parameterExpressions = GetParameterExpressions(variableNames, workflow);
            var sanitizedExpression = expression.ToString().Replace(Percentage, string.Empty);

            if (string.IsNullOrEmpty(sanitizedExpression))
                return sanitizedExpression;

            var workflowVariables = workflow.GetVariables(variableNames);
            var variableValues = workflowVariables.Select(x => x.Value);

            var lambdaExpresion = DynamicExpressionParser.ParseLambda(parameterExpressions.ToArray(), null, sanitizedExpression);
            var expressionValue = lambdaExpresion.Compile().DynamicInvoke(variableValues.ToArray());

            return expressionValue;
        }

        private static IEnumerable<string> GetVariableNames(object inputValue)
        {
            var inputString = inputValue.ToString();
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