using Automaton.Studio.Conductor;
using System;

namespace Automaton.Studio.Events
{
    public class StepEventArgs : EventArgs
    {
        public StepEventArgs(Step activity)
        {
            Activity = activity;
        }

        public Step Activity { get; set; }
    }
}