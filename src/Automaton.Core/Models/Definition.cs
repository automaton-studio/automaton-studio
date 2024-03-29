﻿namespace Automaton.Core.Models;

public class Definition
{
    public string Id { get; set; }

    public string Name { get; set; }

    public List<Step> Steps { get; set; } = new List<Step>();

    public Definition()
    {
        Id = Guid.NewGuid().ToString();
        Name = "Untitled";
    }
}
