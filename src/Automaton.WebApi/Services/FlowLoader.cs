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
    public class FlowLoader : IFlowLoader
    {
        private readonly IServiceProvider serviceProvider;

        public FlowLoader(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Workflow LoadFlow(string source, Func<string, Flow> deserializer)
        {
            var sourceObj = deserializer(source);
            var def = LoadFlow(sourceObj);

            return def;
        }

        public Workflow LoadFlow(Flow flow)
        {
            var worklow = new Workflow
            {
                Id = flow.Id,
                Name = flow.Name,
                StartupDefinitionId = flow.StartupDefinitionId,
            };

            foreach(var definition in flow.Definitions)
            {
                var workflowDefinition = new WorkflowDefinition
                {
                    Id = definition.Id,
                    Steps = ConvertSteps(definition.Steps),
                    DefaultErrorBehavior = definition.DefaultErrorBehavior,
                    DefaultErrorRetryInterval = definition.DefaultErrorRetryInterval,
                    Description = definition.Description
                };

                worklow.Definitions.Add(workflowDefinition);
            }
            
            return worklow;
        }

        private List<WorkflowStep> ConvertSteps(ICollection<Step> source)
        {
            var workflowSteps = new List<WorkflowStep>();

            foreach (var step in source)
            {
                var stepType = FindType(step.Name);
                var targetStep = serviceProvider.GetService(stepType) as WorkflowStep;
                targetStep.Id = step.Id;
                targetStep.Name = step.Name;
                targetStep.ErrorBehavior = step.ErrorBehavior;
                targetStep.RetryInterval = step.RetryInterval;

                AttachInputs(step, stepType, targetStep);
                AttachOutputs(step, stepType, targetStep);

                workflowSteps.Add(targetStep);
            }

            return workflowSteps;
        }

        private void AttachInputs(Step source, Type stepType, WorkflowStep step)
        {
            try
            {
                foreach (var input in source.Inputs)
                {
                    var dataParameter = Expression.Parameter(stepType, input.Key);
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
                        step.Inputs.Add(new ActionParameter<WorkflowStep, object>(acn));
                        continue;
                    }

                    if ((input.Value is IDictionary<string, object>) || (input.Value is IDictionary<object, object>))
                    {
                        var acn = BuildObjectInputAction(input, dataParameter, contextParameter, environmentVarsParameter, stepProperty);
                        step.Inputs.Add(new ActionParameter<WorkflowStep, object>(acn));
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

        private void AttachOutputs(Step source, Type stepType, WorkflowStep step)
        {
            //foreach (var output in source.Outputs)
            //{
            //    var stepParameter = Expression.Parameter(stepType, "step");
            //    var sourceExpr = DynamicExpressionParser.ParseLambda(new[] { stepParameter }, typeof(object), output.Value);

            //    var dataParameter = Expression.Parameter(dataType, "data");


            //    if (output.Key.Contains(".") || output.Key.Contains("["))
            //    {
            //        AttachNestedOutput(output, step, source, sourceExpr, dataParameter);
            //    }
            //    else
            //    {
            //        AttachDirectlyOutput(output, step, dataType, sourceExpr, dataParameter);
            //    }
            //}
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

        private void AttachNestedOutput(KeyValuePair<string, string> output, WorkflowStep step, Step source, LambdaExpression sourceExpr, ParameterExpression dataParameter)
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

        private static Action<WorkflowStep, object, IStepExecutionContext> BuildScalarInputAction(KeyValuePair<string, object> input, ParameterExpression dataParameter, ParameterExpression contextParameter, ParameterExpression environmentVarsParameter, PropertyInfo stepProperty)
        {
            try
            {
                var expr = System.Convert.ToString(input.Key);
                var sourceExpr = DynamicExpressionParser.ParseLambda(new[] { dataParameter, contextParameter, environmentVarsParameter }, typeof(object), expr);

                void acn(WorkflowStep pStep, object pData, IStepExecutionContext pContext)
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

        private static Action<WorkflowStep, object, IStepExecutionContext> BuildObjectInputAction(KeyValuePair<string, object> input, ParameterExpression dataParameter, ParameterExpression contextParameter, ParameterExpression environmentVarsParameter, PropertyInfo stepProperty)
        {
            void acn(WorkflowStep pStep, object pData, IStepExecutionContext pContext)
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
