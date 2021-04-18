using System;
using System.Collections.Generic;

#nullable disable

namespace Automaton.Studio.Entities
{
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
