using Automaton.Core.Enums;

namespace Automaton.Studio.Server.Models
{
    public class Definition
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public WorkflowErrorHandling DefaultErrorBehavior { get; set; }

        public TimeSpan? DefaultErrorRetryInterval { get; set; }

        public List<Step> Steps { get; set; } = new List<Step>();
    }
}
