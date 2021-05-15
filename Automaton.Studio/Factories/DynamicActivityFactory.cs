using AutoMapper;
using Automaton.Activity;
using Automaton.Activity.WriteLine;
using Elsa.Models;
using System;

namespace Automaton.Studio.Factories
{
    public class DynamicActivityFactory
    {
        private readonly IMapper mapper;

        public DynamicActivityFactory(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public DynamicActivity GetDynamicActivity(ActivityDefinition activityDefinition)
        {
            switch (activityDefinition.Type)
            {
                case "WriteLine":
                    return mapper.Map<WriteLineActivity>(activityDefinition);
                default:
                    throw new NotImplementedException(activityDefinition.Type);
            }  
        }
    }
}
