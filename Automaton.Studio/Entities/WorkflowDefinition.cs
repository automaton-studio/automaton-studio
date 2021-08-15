using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Automaton.Studio.Entities
{
    [Table("WorkflowDefinitions", Schema = "Elsa")]
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
        public string Tag { get; set; }
    }
}
