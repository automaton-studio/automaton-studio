using Automaton.Core.Enums;
using System.Dynamic;

namespace Automaton.WebApi.Models
{
    public class Step
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public WorkflowErrorHandling? ErrorBehavior { get; set; }

        public TimeSpan? RetryInterval { get; set; }

        public ExpandoObject Inputs { get; set; } = new ExpandoObject();

        public string? NextStepId { get; set; }

        public List<Step> Children { get; set; } = new List<Step>();
    }
}
