using Automaton.Core.Models;
using MediatR;

namespace Automaton.Core.Events
{
    public class SetVariableNotification: INotification
    {
        public StepVariable Variable { get; private set; }

        public SetVariableNotification(StepVariable variable)
        {
            Variable = variable;
        }
    }
}
