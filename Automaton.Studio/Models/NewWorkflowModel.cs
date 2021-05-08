using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Models
{
    public class NewWorkflowModel
    {
        [Required]
        public string? Name { get; set; }
    }
}
