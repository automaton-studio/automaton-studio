using AutoMapper;
using Automaton.Studio.Activity;
using Automaton.Studio.Metadata;
using Elsa.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Factories
{
    public class ActivityFactory
    {
        private readonly IMapper mapper;
        private readonly IServiceProvider serviceProvider;
        private readonly IActivityTypeDescriber describesActivityType;
        private readonly AutomatonOptions automatonOptions;

        public ActivityFactory(
            IMapper mapper,
            IActivityTypeDescriber describesActivityType,
            IServiceProvider serviceProvider,
            AutomatonOptions automatonOptions)
        {
            this.mapper = mapper;
            this.serviceProvider = serviceProvider;
            this.describesActivityType = describesActivityType;
            this.automatonOptions = automatonOptions;
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

        public StudioActivity GetStudioActivity(ActivityDefinition activityDefinition)
        {
            var activityType = automatonOptions.GetElsaActivityType(activityDefinition.Type);
            var studioActivity = serviceProvider.GetService(activityType) as StudioActivity;
            mapper.Map(activityDefinition, studioActivity);

            return studioActivity;
        }

        public StudioActivity GetStudioActivity(string name)
        {
            var activityType = automatonOptions.GetStudioActivityType(name);
            return serviceProvider.GetService(activityType) as StudioActivity;
        }
    }
}
