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
        private const string StepsAssembly = "Automaton.Studio";

        private IDictionary<string, SolutionStep> solutionSteps;
        private IDictionary<string, Type> solutionTypes;
        private IStepTypeDescriptor stepTypeDescriber;

        public StepFactory(IStepTypeDescriptor stepTypeDescriber)
        {
            this.stepTypeDescriber = stepTypeDescriber;
            solutionSteps = new Dictionary<string, SolutionStep>();
            solutionTypes = new Dictionary<string, Type>();

            var assembly = Assembly.Load(StepsAssembly);
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
                Type = stepDescriptor.Type,
                Description = stepDescriptor.Description,
                Category = stepDescriptor.Category,
                Icon = stepDescriptor.Icon
            };

            solutionSteps.Add(solutionStep.Name, solutionStep);
            solutionTypes.Add(solutionStep.Name, stepType);
        }

        public Step GetStep(string name)
        {
            var descriptor = stepTypeDescriber.Describe(solutionTypes[name]);
            var step = Activator.CreateInstance(solutionTypes[name], descriptor) as Step;

            return step;
        }
    }
}
