using Automaton.Studio.Domain;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.CustomStepDesigner
{
    public partial class CustomStepProperties : ComponentBase
    {
        [Parameter] public CustomStep CustomStep { get; set; } = new();

        public CustomStepProperties()
        {
        }       
    }
}
