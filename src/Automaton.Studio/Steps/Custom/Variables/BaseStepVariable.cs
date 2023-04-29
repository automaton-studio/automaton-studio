using Automaton.Core.Models;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Steps.Custom.Variables
{
    public abstract class BaseStepVariable : ComponentBase
    {
        [Parameter] public StepVariable Variable { get; set; }  
    }
}
