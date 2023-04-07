using Automaton.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Steps.Custom.Variables
{
    public abstract class StepVariable : ComponentBase
    {
        [Parameter] public CustomStepVariable Variable { get; set; }  
    }
}
