﻿namespace Automaton.Core.Models;

public class StepVariable
{
    public string? Key { get; set; }
    public string? OldName { get; set; }
    public string? Name { get; set; }
    public object? Value { get; set; }

    public bool VariableNameIsTheSame()
    {
        return string.Compare(OldName, Name, true) == 0;
    }
}