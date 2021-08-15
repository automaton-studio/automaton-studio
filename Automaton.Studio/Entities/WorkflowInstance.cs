using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Automaton.Studio.Entities
{
    [Table("WorkflowInstances", Schema = "Elsa")]
    public partial class WorkflowInstance
    {
        public string Id { get; set; }
        public string DefinitionId { get; set; }
        public string TenantId { get; set; }
        public int Version { get; set; }
        public int WorkflowStatus { get; set; }
        public string CorrelationId { get; set; }
        public string ContextType { get; set; }
        public string ContextId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? LastExecutedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public DateTimeOffset? CancelledAt { get; set; }
        public DateTimeOffset? FaultedAt { get; set; }
        public string Data { get; set; }
    }
}
