#nullable disable

using Automaton.Studio.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace Automaton.Studio.Server.Entities;

public class Schedule
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public Guid FlowId { get; set; }

    [Required]
    public string RunnerIds { get; set; }

    [Required]
    public string CronRecurrence { get; set; }

    public virtual IEnumerable<ScheduleUser> ScheduleUsers { get; set; }
}
