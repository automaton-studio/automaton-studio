using Automaton.Studio.Domain;
using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages.FlowDesigner.Components.Drawer;

public partial class FlowSettings : ComponentBase
{
    [CascadingParameter]
    private StudioFlow Flow { get; set; }

    private bool loading = false;
    private FluentValidationValidator fluentValidationValidator;

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task OnFinish(EditContext editContext)
    {
        try
        {
            loading = true;

            if (fluentValidationValidator.Validate(options => options.IncludeAllRuleSets()))
            {

            }
        }
        catch (Exception ex)
        {
           // await MessageService.Error("User profile update failed");
        }
        finally
        {
            loading = false;
        }
    }

    private void OnFinishFailed(EditContext editContext)
    {
        // Do nothing if form validation fails
    }
}
