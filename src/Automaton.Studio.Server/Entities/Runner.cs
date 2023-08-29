#nullable disable

using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Entities;

public class Runner
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string ConnectionId { get; set; }

    public virtual IEnumerable<RunnerUser> RunnerUsers { get; set; }
}
