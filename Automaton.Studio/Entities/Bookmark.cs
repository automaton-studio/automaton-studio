using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Automaton.Studio.Entities
{
    [Table("Bookmarks", Schema = "Elsa")]
    public partial class Bookmark
    {
        public string Id { get; set; }
        public string TenantId { get; set; }
        public string Hash { get; set; }
        public string Model { get; set; }
        public string ModelType { get; set; }
        public string ActivityType { get; set; }
        public string ActivityId { get; set; }
        public string WorkflowInstanceId { get; set; }
    }
}
