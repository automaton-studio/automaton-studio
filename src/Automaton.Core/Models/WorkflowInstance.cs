using Automaton.Core.Enums;

namespace WorkflowCore.Models
{
    public class WorkflowInstance
    {
        public string Id { get; set; }
                
        public string WorkflowDefinitionId { get; set; }

        public int Version { get; set; }

        public string Description { get; set; }

        public string Reference { get; set; }

        public long? NextExecution { get; set; }

        public WorkflowStatus Status { get; set; }

        public object Data { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? CompleteTime { get; set; }
    }
}
