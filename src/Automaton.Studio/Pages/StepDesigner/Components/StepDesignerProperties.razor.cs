using AntDesign;
using Automaton.Studio.Domain;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.StepDesigner.Components
{
    public partial class StepDesignerProperties : ComponentBase
    {
        [Parameter] public CustomStep CustomStep { get; set; } = new CustomStep();

        private Form<CustomStep> form;
        private FluentValidationValidator fluentValidationValidator;
    }
}
