using System;
using System.Collections.Generic;

#nullable disable

namespace Automaton.Studio.Entities
{
    public partial class WorkflowDefinition
    {
        public string Id { get; set; }
        public string DefinitionId { get; set; }
        public string TenantId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public int Version { get; set; }
        public bool IsSingleton { get; set; }
        public int PersistenceBehavior { get; set; }
        public bool DeleteCompletedInstances { get; set; }
        public bool IsPublished { get; set; }
        public bool IsLatest { get; set; }
        public string Data { get; set; }

        public virtual IEnumerable<FlowWorkflow> FlowWorkflows { get; set; }
    }
}
