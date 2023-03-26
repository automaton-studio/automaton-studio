﻿namespace Automaton.Core.Models;

public class CustomStepVariable
{
    public string Name { get; set; }
    public string Type { get; set; }
    public object? Value { get; set; }
    public object? DefaultValue { get; set; }
    public string? Description { get; set; }
    public object? Data { get; set; }
}
