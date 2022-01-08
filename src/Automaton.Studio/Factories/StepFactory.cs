using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Extensions;
using Automaton.Studio.Models;
using Automaton.Studio.Steps.EmitLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Automaton.Studio.Factories
{
    public class StepFactory
    {
        private IDictionary<string, SolutionStep> solutionSteps;
        private IActivityTypeDescriber activityTypeDescriber;

        public StepFactory(IActivityTypeDescriber activityTypeDescriber)
        {
            this.activityTypeDescriber = activityTypeDescriber;
            solutionSteps = new Dictionary<string, SolutionStep>();
            var assembly = Assembly.Load("Automaton.Studio");
            AddActivitiesFrom(assembly);
        }

        public IEnumerable<SolutionStep> GetSteps()
        {
            return solutionSteps.Values;
        }

        public void AddActivitiesFrom(Assembly assembly) => AddActivitiesFrom(new[] { assembly });

        public void AddActivitiesFrom(IEnumerable<Assembly> assemblies)
        {
            var types = assemblies.SelectMany(x => x.GetAllWithBaseClass<Step>()).Where(x => !x.IsAbstract);

            foreach (var type in types)
            {
                AddActivity(type);
            }
        }

        public void AddActivity(Type activityType)
        {
            var activityDescription = activityTypeDescriber.Describe(activityType);

            var solutionStep = new SolutionStep 
            { 
                Name = activityDescription.Name, 
                Type = activityDescription.Name,
                Description = activityDescription.Description,
                Category = activityDescription.Category,
                Icon = activityDescription.Icon
            };

            solutionSteps.Add(solutionStep.Name, solutionStep);
        }

        public Step GetStep(string name)
        {
            var step = new EmitLogActivity { Name = name };
            return step;
        }
    }
}
