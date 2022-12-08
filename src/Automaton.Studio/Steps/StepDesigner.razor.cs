using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Extensions;
using Automaton.Studio.Resources;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps;

public partial class StepDesigner : ComponentBase
{
    [Parameter]
    public StudioStep Step { get; set; }

    [Parameter] 
    public RenderFragment ChildContent { get; set; }

    [Inject] 
    private ModalService ModalService { get; set; } = default!;

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private async Task OnSettings(StudioStep step)
    {
        var stepSettings = new StepSettingsModel
        {
            DisplayName = Step.DisplayName,
            Description = Step.Description
        };

        var settingsDialog = await ModalService.CreateModalAsync<StepSettingsDialog, StepSettingsModel>
        (
            new ModalOptions { Title = Labels.StepSettings }, stepSettings
        );

        settingsDialog.OnOk = () =>
        {
            Step.DisplayName = stepSettings.DisplayName;
            Step.Description = stepSettings.Description;

            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private async Task OnEdit(StudioStep step)
    {
        var result = await step.DisplayPropertiesDialog(ModalService);

        result.OnOk = () => {

            StateHasChanged();

            return Task.CompletedTask;
        };
    }

    private void OnDelete(StudioStep step)
    {
        step.Definition.DeleteStep(step);
    }
}
