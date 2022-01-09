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
        private IStepTypeDescriptor stepTypeDescriber;

        public StepFactory(IStepTypeDescriptor stepTypeDescriber)
        {
            this.stepTypeDescriber = stepTypeDescriber;
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
                AddStep(type);
            }
        }

        public void AddStep(Type stepType)
        {
            var stepDescriptor = stepTypeDescriber.Describe(stepType);

            var solutionStep = new SolutionStep 
            { 
                Name = stepDescriptor.Name, 
                Type = stepDescriptor.Name,
                Description = stepDescriptor.Description,
                Category = stepDescriptor.Category,
                Icon = stepDescriptor.Icon
            };

            solutionSteps.Add(solutionStep.Name, solutionStep);
        }

        public Step GetStep(string name)
        {
            var descriptor = stepTypeDescriber.Describe(typeof(EmitLogStep));
            var step = new EmitLogStep(descriptor) { Name = name };

            return step;
        }
    }
}
