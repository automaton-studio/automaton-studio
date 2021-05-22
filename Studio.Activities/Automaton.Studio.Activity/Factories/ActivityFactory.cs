using Automaton.Studio.Activity;
using Automaton.Studio.Activity.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities.Factories
{
    public class ActivityFactory
    {
        private readonly IDescribesActivityType describesActivityType;
        private AutomatonOptions automatonOptions;

        public ActivityFactory(IDescribesActivityType describesActivityType,
            AutomatonOptions automatonOptions,
            IServiceProvider serviceProvider)
        {
            this.automatonOptions = automatonOptions;
            this.describesActivityType = describesActivityType;
        }

        private IEnumerable<Type> GetActivityTypes() => automatonOptions.Types;

        public IEnumerable<ActivityDescriptor> GetActivityDescriptors()
        {
            var activityDescriptors = new List<ActivityDescriptor>();

            foreach (var types in GetActivityTypes())
            {
                var activityDescriptor = CreateActivityType(types);
                activityDescriptors.Add(activityDescriptor);
            }

            return activityDescriptors;
        }

        private ActivityDescriptor? CreateActivityType(Type activityType)
        {
            //var foo = serviceProvider.GetService(activityType);

            var info = describesActivityType.Describe(activityType);

            return info??default;
        }

        public DynamicActivity GetActivityByDefinition(ActivityDefinition activityDefinition)
        {
            var activity = GetActivityByType(activityDefinition.Type);

            return activity;
        }

        public DynamicActivity GetActivityByType(string activityType)
        {
            return GetInstance(activityType);
        }

        public DynamicActivity GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return Activator.CreateInstance(t) as DynamicActivity;
        }
    }
}
