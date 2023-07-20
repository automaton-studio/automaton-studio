using Automaton.Core.Models;
using MediatR;

namespace Automaton.Core.Events
{
    public class SetVariableNotification: INotification
    {
        public StepVariable Variable { get; set; }
    }
}
