using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Automaton.Studio.Entities
{
    [Table("WorkflowExecutionLogRecords", Schema = "Elsa")]
    public partial class WorkflowExecutionLogRecord
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string WorkflowInstanceId { get; set; }
        public string ActivityId { get; set; }
        public string ActivityType { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
    }
}
