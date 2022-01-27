using Automaton.Studio.Domain;
using System;

namespace Automaton.Studio.Events
{
    public class StepEventArgs : EventArgs
    {
        public Step Step { get; set; }

        public StepEventArgs(Step step)
        {
            Step = step;
        }
    }
}