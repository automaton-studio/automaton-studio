using Automaton.Core.Models;

namespace Automaton.Studio.Events
{
    public class VariableUpdateNotification: INotification
    {
        public StepVariable Variable { get; private set; }

        public VariableUpdateNotification(StepVariable variable)
        {
            Variable = variable;
        }
    }
}
