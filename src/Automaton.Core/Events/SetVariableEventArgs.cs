using Automaton.Core.Models;

namespace Automaton.Core.Events
{
    public class SetVariableEventArgs : EventArgs
    {
        public StepVariable Variable { get; set; }

        public SetVariableEventArgs(StepVariable variable)
        {
            Variable = variable;
        }
    }
}
