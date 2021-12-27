using AutoMapper;
using Automaton.Studio.Conductor;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Factories
{
    public class StepFactory
    {
        private readonly IMapper mapper;
        private readonly IServiceProvider serviceProvider;

        public StepFactory(
            IMapper mapper,
            IServiceProvider serviceProvider)
        {
            this.mapper = mapper;
            this.serviceProvider = serviceProvider;
        }

        public IEnumerable<Step> GetSteps()
        {
            var steps = new List<Step>();

            foreach (var types in GetActivityTypes())
            {
                var activityDescriptor = CreateActivity(types);
                steps.Add(activityDescriptor);
            }

            return steps;
        }

        public Step GetStep(string name)
        {
            return new Step { StepType = "EmitLog" };
        }

        private Step CreateActivity(string type)
        {
            return new Step { StepType = "EmitLog" };
        }

        private IEnumerable<string> GetActivityTypes() => new List<string> { "EmitLog" };  
    }
}
