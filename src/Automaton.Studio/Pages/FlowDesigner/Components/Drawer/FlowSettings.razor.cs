
using AntDesign;
using Automaton.Studio.Domain;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner.Components.Drawer;

public partial class FlowSettings : ComponentBase
{
    [CascadingParameter]
    private StudioFlow Flow { get; set; }

    private FluentValidationValidator fluentValidationValidator;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    public async Task Submit()
    {
        if (fluentValidationValidator.Validate(options => options.IncludeAllRuleSets()))
        {
            
        }
    }

    public async Task Cancel()
    {
    }
}
