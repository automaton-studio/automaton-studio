using Automaton.Core.Enums;

namespace Automaton.Core.Models
{
    public class WorkflowDefinition
    {
        public string Id { get; set; }

        public string Description { get; set; }

        public IEnumerable<WorkflowStep> Steps => StepsDictionary.Values;

        public IDictionary<string, WorkflowStep> StepsDictionary { get; set; } = new Dictionary<string, WorkflowStep>();

        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

        public WorkflowStep GetFirstStep()
        {
            return StepsDictionary.First().Value;
        }
    }
}
