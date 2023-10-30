using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Automaton.Studio.Steps;

public partial class StepDesigner : ComponentBase
{
    [Parameter] public StudioStep Step { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Inject] private ModalService ModalService { get; set; }
    [Inject] private IMediator Mediator { get; set; }

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

    private async Task OnDelete(StudioStep step)
    {
        var selectedSteps = step.Definition.Steps.Where(x => x.IsSelected());

        if (selectedSteps.Count() > 1)
        {
           await DeleteSelectedSteps(step.Definition);
        }
        else
        {
            await DeleteStep(step);
        }    
    }

    private async Task DeleteStep(StudioStep step)
    {
        var newVariableDialog = await ModalService.ConfirmAsync(new ConfirmOptions()
        {
            Title = Labels.DeleteStep,
            Content = Labels.DeleteStepConfirmation,
            OnOk = e =>
            {
                step.Definition.DeleteStep(step);

                Mediator.Publish(new FlowUpdateNotification());

                return Task.CompletedTask;
            }
        });
    }

    private async Task DeleteSelectedSteps(StudioDefinition definition)
    {
        var newVariableDialog = await ModalService.ConfirmAsync(new ConfirmOptions()
        {
            Title = Labels.DeleteSteps,
            Content = Labels.DeleteStepsConfirmation,
            OnOk = e =>
            {
                definition.DeleteSelectedSteps();

                Mediator.Publish(new FlowUpdateNotification());

                return Task.CompletedTask;
            }
        });
    }

    private async Task OnKeyDown(KeyboardEventArgs e, StudioStep step)
    {
        if (e.Key == "Delete")
        {
            await OnDelete(step);
        }
    } 
}
