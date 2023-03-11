using Automaton.Studio.Domain;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.StepDesigner
{
    public partial class StepDesignerProperties : ComponentBase
    {
        [Parameter] public CustomStep CustomStep { get; set; } = new();

        public StepDesignerProperties()
        {
        }       
    }
}
