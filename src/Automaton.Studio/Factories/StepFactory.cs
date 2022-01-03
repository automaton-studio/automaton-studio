using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Models;
using System;
using System.Collections.Generic;

namespace Automaton.Studio.Factories
{
    public class StepFactory
    {
        private IDictionary<string, SolutionStep> solutionSteps;

        public StepFactory()
        {
        }

        public IEnumerable<SolutionStep> GetSteps()
        {
            if (solutionSteps == null)
            {
                solutionSteps = new Dictionary<string, SolutionStep>();

                var console = new SolutionStep { Name = "Console" };
                var writeLine = new SolutionStep { Name = "WriteLine", Type = "WriteLine" };

                console.AddStep(writeLine);
                solutionSteps.Add(console.Name, console);
            }

            return solutionSteps.Values;
        }

        public Step GetStep(string name)
        {
            var step = new Step { Name = name };
            return step;
        }
    }
}
