using AutoMapper;
using Automaton.Studio.Activity;
using Automaton.Studio.Activity.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Activities.Factories
{
    public class ActivityFactory
    {
        private readonly IMapper mapper;
        private readonly IServiceProvider serviceProvider;
        private readonly IDescribesActivityType describesActivityType;
        private readonly AutomatonOptions automatonOptions;

        public ActivityFactory(
            IMapper mapper,
            IDescribesActivityType describesActivityType,
            AutomatonOptions automatonOptions,
            IServiceProvider serviceProvider)
        {
            this.mapper = mapper;
            this.serviceProvider = serviceProvider;
            this.automatonOptions = automatonOptions;
            this.describesActivityType = describesActivityType;
        }

        public IEnumerable<ActivityDescriptor> GetActivityDescriptors()
        {
            var activityDescriptors = new List<ActivityDescriptor>();

            foreach (var types in GetActivityTypes())
            {
                var activityDescriptor = CreateActivityDescriptor(types);
                activityDescriptors.Add(activityDescriptor);
            }

            return activityDescriptors;
        }

        private IEnumerable<Type> GetActivityTypes() => automatonOptions.AutomatonActivities;

        private ActivityDescriptor? CreateActivityDescriptor(Type activityType)
        {
            var info = describesActivityType.Describe(activityType);

            return info??default;
        }

        public DynamicActivity GetStudioActivity(ActivityDefinition activityDefinition)
        {
            var activityType = automatonOptions.GetElsaActivity(activityDefinition.Type);
            var studioActivity = serviceProvider.GetService(activityType) as DynamicActivity;
            mapper.Map<ActivityDefinition, DynamicActivity>(activityDefinition, studioActivity);

            return studioActivity;
        }

        public DynamicActivity GetStudioActivity(string name)
        {
            var activityType = automatonOptions.GetAutomatonActivity(name);
            return serviceProvider.GetService(activityType) as DynamicActivity;
        }
    }
}
