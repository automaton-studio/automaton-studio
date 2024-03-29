﻿using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Automaton.Studio.Steps.Sequence;

public partial class SequenceDesigner : ComponentBase
{
    [Parameter] public SequenceStep Step { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Inject] private ModalService ModalService { get; set; } = default!;
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
        var newVariableDialog = await ModalService.ConfirmAsync(new ConfirmOptions()
        {
            Title = Labels.DeleteStep,
            Content = Labels.DeleteStepConfirmation,
            OnOk = e =>
            {
                Step.Delete();

                Mediator.Publish(new FlowUpdateNotification());

                return Task.CompletedTask;
            }
        });  
    }

    private void ToggleContent()
    {
        Step.Collapsed = !Step.Collapsed;
        Step.SequenceEndStep.Collapsed = !Step.SequenceEndStep.Collapsed;

        SetChildrenColapse(Step.Collapsed);
    }

    private void SetChildrenColapse(bool collapsed)
    {
        var children = Step.GetChildrenExcludingEndStep().Where(x => x.ParentId == Step.Id || (x.Parent != null && !x.Parent.Collapsed));

        foreach (var child in children)
        {
            child.Hidden = collapsed;
        }
    }

    private string GetExpandCollapseIcon()
    {
        return Step.Collapsed ? "right" : "down";
    }

    private async Task OnKeyDown(KeyboardEventArgs e, StudioStep step)
    {
        if (e.Key == "Delete")
        {
            await OnDelete(step);
        }
    }
}
