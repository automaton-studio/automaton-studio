using MediatR;

namespace Automaton.Core.Events
{
    public class ExecuteStepNotification : INotification
    {
        public string StepId { get; private set; }

        public ExecuteStepNotification(string stepId)
        {
            StepId = stepId;
        }
    }
}
