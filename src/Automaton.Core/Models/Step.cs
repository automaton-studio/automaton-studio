﻿namespace Automaton.Core.Models;

public class Step
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string DisplayName { get; set; }

    public string? Description { get; set; }

    public string Type { get; set; }

    public string? MoreInfo { get; set; }

    public string? Category { get; set; }

    public bool VisibleInExplorer { get; set; }

    public string Icon { get; set; }

    public IDictionary<string, StepVariable> Inputs { get; set; } = new Dictionary<string, StepVariable>();

    public IDictionary<string, StepVariable> Outputs { get; set; } = new Dictionary<string, StepVariable>();

    public string? NextStepId { get; set; }

    public string? ParentId { get; set; }
}
