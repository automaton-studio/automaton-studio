using Automaton.Core.Exceptions;
using Automaton.Core.Interfaces;
using Automaton.Core.Models;
using Automaton.WebApi.Interfaces;
using Automaton.WebApi.Models;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Automaton.WebApi.Services
{
    public class DefinitionLoader : IDefinitionLoader
    {
        private readonly IServiceProvider serviceProvider;

        public DefinitionLoader(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public WorkflowDefinition LoadDefinition(string source, Func<string, Definition> deserializer)
        {
            var sourceObj = deserializer(source);
            var def = LoadDefinition(sourceObj);
            return def;
        }

        public WorkflowDefinition LoadDefinition(Definition source)
        {
            var dataType = typeof(object);
            if (!string.IsNullOrEmpty(source.DataType))
                dataType = FindType(source.DataType);

            var result = new WorkflowDefinition
            {
                Id = source.Id,
                Version = source.Version,
                Steps = ConvertSteps(source.Steps, dataType),
                DefaultErrorBehavior = source.DefaultErrorBehavior,
                DefaultErrorRetryInterval = source.DefaultErrorRetryInterval,
                Description = source.Description,
                DataType = dataType
            };

            return result;
        }


        private List<WorkflowStep> ConvertSteps(ICollection<Step> source, Type dataType)
        {
            var result = new List<WorkflowStep>();
            int i = 0;
            var stack = new Stack<Step>(source.Reverse<Step>());
            var parents = new List<Step>();
            var compensatables = new List<Step>();

            while (stack.Count > 0)
            {
                var nextStep = stack.Pop();

                var stepType = FindType(nextStep.Name);

                Type containerType;

                var targetStep = serviceProvider.GetService(stepType) as WorkflowStep;

                if (!string.IsNullOrEmpty(nextStep.CancelCondition))
                {
                    var cancelExprType = typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(dataType, typeof(bool)));
                    var dataParameter = Expression.Parameter(dataType, "data");
                    var cancelExpr = DynamicExpressionParser.ParseLambda(new[] { dataParameter }, typeof(bool), nextStep.CancelCondition);
                    targetStep.CancelCondition = cancelExpr;
                }

                targetStep.Id = nextStep.Id;
                targetStep.Name = nextStep.Name;
                targetStep.ErrorBehavior = nextStep.ErrorBehavior;
                targetStep.RetryInterval = nextStep.RetryInterval;

                AttachInputs(nextStep, dataType, stepType, targetStep);
                AttachOutputs(nextStep, dataType, stepType, targetStep);

                if (nextStep.Do != null)
                {
                    foreach (var branch in nextStep.Do)
                    {
                        foreach (var child in branch.Reverse<Step>())
                            stack.Push(child);
                    }

                    if (nextStep.Do.Count > 0)
                        parents.Add(nextStep);
                }

                if (nextStep.CompensateWith != null)
                {
                    foreach (var compChild in nextStep.CompensateWith.Reverse<Step>())
                        stack.Push(compChild);

                    if (nextStep.CompensateWith.Count > 0)
                        compensatables.Add(nextStep);
                }

                result.Add(targetStep);

                i++;
            }

            foreach (var step in result)
            {
                if (result.Any(x => x.Id != step.Id))
                    throw new WorkflowDefinitionLoadException($"Duplicate step Id {step.Id}");

            }

            foreach (var parent in parents)
            {
                var target = result.Single(x => x.Id == parent.Id);
                foreach (var branch in parent.Do)
                {
                    var childTags = branch.Select(x => x.Id).ToList();
                    target.Children.AddRange(result
                        .Where(x => childTags.Contains(x.Id))
                        .OrderBy(x => x.Id)
                        .Select(x => x.Id)
                        .Take(1)
                        .ToList());
                }
            }

            foreach (var item in compensatables)
            {
                var target = result.Single(x => x.Id == item.Id);
                var tag = item.CompensateWith.Select(x => x.Id).FirstOrDefault();
                if (tag != null)
                {
                    var compStep = result.FirstOrDefault(x => x.Id == tag);
                    if (compStep != null)
                        target.CompensationStepId = compStep.Id;
                }
            }

            return result;
        }

        private void AttachInputs(Step source, Type dataType, Type stepType, WorkflowStep step)
        {
            try
            {
                foreach (var input in source.Inputs)
                {
                    var dataParameter = Expression.Parameter(dataType, "data");
                    var contextParameter = Expression.Parameter(typeof(IStepExecutionContext), "context");
                    var environmentVarsParameter = Expression.Parameter(typeof(IDictionary), "environment");
                    var stepProperty = stepType.GetProperty(input.Key);

                    if (stepProperty == null)
                    {
                        throw new ArgumentException($"Unknown property for input {input.Key} on {source.Id}");
                    }

                    if (input.Value is string)
                    {
                        var acn = BuildScalarInputAction(input, dataParameter, contextParameter, environmentVarsParameter, stepProperty);
                        step.Inputs.Add(new ActionParameter<IStepBody, object>(acn));
                        continue;
                    }

                    if ((input.Value is IDictionary<string, object>) || (input.Value is IDictionary<object, object>))
                    {
                        var acn = BuildObjectInputAction(input, dataParameter, contextParameter, environmentVarsParameter, stepProperty);
                        step.Inputs.Add(new ActionParameter<IStepBody, object>(acn));
                        continue;
                    }

                    throw new ArgumentException($"Unknown type for input {input.Key} on {source.Id}");
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private void AttachOutputs(Step source, Type dataType, Type stepType, WorkflowStep step)
        {
            foreach (var output in source.Outputs)
            {
                var stepParameter = Expression.Parameter(stepType, "step");
                var sourceExpr = DynamicExpressionParser.ParseLambda(new[] { stepParameter }, typeof(object), output.Value);

                var dataParameter = Expression.Parameter(dataType, "data");


                if(output.Key.Contains(".") || output.Key.Contains("["))
                {
                    AttachNestedOutput(output, step, source, sourceExpr, dataParameter);
                }else
                {
                    AttachDirectlyOutput(output, step, dataType, sourceExpr, dataParameter);
                }
            }
        }

        private void AttachDirectlyOutput(KeyValuePair<string, string> output, WorkflowStep step, Type dataType, LambdaExpression sourceExpr, ParameterExpression dataParameter)
        {
            Expression targetProperty;

            // Check if our datatype has a matching property
            var propertyInfo = dataType.GetProperty(output.Key);
            if (propertyInfo != null)
            {
                targetProperty = Expression.Property(dataParameter, propertyInfo);
                var targetExpr = Expression.Lambda(targetProperty, dataParameter);
                step.Outputs.Add(new MemberMapParameter(sourceExpr, targetExpr));
            }
            else
            {
                // If we did not find a matching property try to find a Indexer with string parameter
                propertyInfo = dataType.GetProperty("Item");
                targetProperty = Expression.Property(dataParameter, propertyInfo, Expression.Constant(output.Key));

                Action<IStepBody, object> acn = (pStep, pData) =>
                {
                    object resolvedValue = sourceExpr.Compile().DynamicInvoke(pStep); ;
                    propertyInfo.SetValue(pData, resolvedValue, new object[] { output.Key });
                };

                step.Outputs.Add(new ActionParameter<IStepBody, object>(acn));
            }
        }

        private void AttachNestedOutput( KeyValuePair<string, string> output, WorkflowStep step, Step source, LambdaExpression sourceExpr, ParameterExpression dataParameter)
        {
            PropertyInfo propertyInfo = null;
            var paths = output.Key.Split('.');
         
            Expression targetProperty = dataParameter;

            bool hasAddOutput = false;

            foreach (string propertyName in paths)
            {
                if (hasAddOutput)
                {
                    throw new ArgumentException($"Unknown property for output {output.Key} on {source.Id}");
                }

                if (targetProperty == null)
                {
                    break;
                }

                if (propertyName.Contains("["))
                {
                    String[] items = propertyName.Split('[');

                    if (items.Length != 2)
                    {
                        throw new ArgumentException($"Unknown property for output {output.Key} on {source.Id}");
                    }

                    items[1] = items[1].Trim().TrimEnd(']').Trim().Trim('"');

                    MemberExpression memberExpression = Expression.Property(targetProperty, items[0]);

                    if (memberExpression == null)
                    {
                        throw new ArgumentException($"Unknown property for output {output.Key} on {source.Id}");
                    }
                    propertyInfo = ((PropertyInfo)memberExpression.Member).PropertyType.GetProperty("Item");

                    Action<IStepBody, object> acn = (pStep, pData) =>
                    {
                        var targetExpr = Expression.Lambda(memberExpression, dataParameter);
                        object data = targetExpr.Compile().DynamicInvoke(pData);
                        object resolvedValue = sourceExpr.Compile().DynamicInvoke(pStep); ;
                        propertyInfo.SetValue(data, resolvedValue, new object[] { items[1] });
                    };

                    step.Outputs.Add(new ActionParameter<IStepBody, object>(acn));
                    hasAddOutput = true;
                }
                else
                {
                    try
                    {
                        targetProperty = Expression.Property(targetProperty, propertyName);
                    }
                    catch
                    {
                        targetProperty = null;
                        break;
                    }
                }
            }

            if (targetProperty != null && !hasAddOutput)
            {
                var targetExpr = Expression.Lambda(targetProperty, dataParameter);
                step.Outputs.Add(new MemberMapParameter(sourceExpr, targetExpr));
            }
        }

        private Type FindType(string name)
        {
            return Type.GetType($"Automaton.Steps.{name}, Automaton.Steps", true, true);
        }

        private static Action<IStepBody, object, IStepExecutionContext> BuildScalarInputAction(KeyValuePair<string, object> input, ParameterExpression dataParameter, ParameterExpression contextParameter, ParameterExpression environmentVarsParameter, PropertyInfo stepProperty)
        {
            try
            {
                var expr = System.Convert.ToString(input.Value);
                var sourceExpr = DynamicExpressionParser.ParseLambda(new[] { dataParameter, contextParameter, environmentVarsParameter }, typeof(object), expr);

                void acn(IStepBody pStep, object pData, IStepExecutionContext pContext)
                {
                    object resolvedValue = sourceExpr.Compile().DynamicInvoke(pData, pContext, Environment.GetEnvironmentVariables());
                    if (stepProperty.PropertyType.IsEnum)
                        stepProperty.SetValue(pStep, Enum.Parse(stepProperty.PropertyType, (string)resolvedValue, true));
                    else
                    {
                        if ((resolvedValue != null) && (stepProperty.PropertyType.IsAssignableFrom(resolvedValue.GetType())))
                            stepProperty.SetValue(pStep, resolvedValue);
                        else
                            stepProperty.SetValue(pStep, System.Convert.ChangeType(resolvedValue, stepProperty.PropertyType));
                    }
                }
                return acn;
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        private static Action<IStepBody, object, IStepExecutionContext> BuildObjectInputAction(KeyValuePair<string, object> input, ParameterExpression dataParameter, ParameterExpression contextParameter, ParameterExpression environmentVarsParameter, PropertyInfo stepProperty)
        {
            void acn(IStepBody pStep, object pData, IStepExecutionContext pContext)
            {
                var stack = new Stack<JObject>();
                var destObj = JObject.FromObject(input.Value);
                stack.Push(destObj);

                while (stack.Count > 0)
                {
                    var subobj = stack.Pop();
                    foreach (var prop in subobj.Properties().ToList())
                    {
                        if (prop.Name.StartsWith("@"))
                        {
                            var sourceExpr = DynamicExpressionParser.ParseLambda(new[] { dataParameter, contextParameter, environmentVarsParameter }, typeof(object), prop.Value.ToString());
                            object resolvedValue = sourceExpr.Compile().DynamicInvoke(pData, pContext, Environment.GetEnvironmentVariables());
                            subobj.Remove(prop.Name);
                            subobj.Add(prop.Name.TrimStart('@'), JToken.FromObject(resolvedValue));
                        }
                    }

                    foreach (var child in subobj.Children<JObject>())
                        stack.Push(child);
                }

                stepProperty.SetValue(pStep, destObj);
            }
            return acn;
        }

    }
}
