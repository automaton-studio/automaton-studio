using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.Resources;
using Automaton.Studio.Services;
using MediatR;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Automaton.Studio.Steps.Sequence;

public partial class SequenceDesigner : ComponentBase
{
    private const int DefaultStepMargin = 10;

    [Parameter] public SequenceStep Step { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    [Inject] private ModalService ModalService { get; set; } = default!;
    [Inject] private ConfigurationService ConfigurationService { get; set; }
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

    private void OnDelete(StudioStep step)
    {
        var sequenceStepIndex = Step.Definition.Steps.IndexOf(Step);
        var endSequenceStepIndex = Step.Definition.Steps.IndexOf(Step.SequenceEndStep);
        var count = endSequenceStepIndex - sequenceStepIndex;

        step.Definition.DeleteSteps(sequenceStepIndex, count + 1);

        Mediator.Publish(new FlowUpdateNotification());
    }

    private void ToggleContent()
    {
        Step.Collapsed = !Step.Collapsed;
        Step.SequenceEndStep.Collapsed = !Step.SequenceEndStep.Collapsed;

        SetChildrenColapse(Step.Collapsed);
    }

    private void SetChildrenColapse(bool collapsed)
    {
        var children = Step.GetChildren().Where(x => x.ParentId == Step.Id || (x.Parent != null && !x.Parent.Collapsed));

        foreach (var child in children)
        {
            child.Hidden = collapsed;
        }
    }

    private string GetExpandCollapseIcon()
    {
        return Step.Collapsed ? "right" : "down";
    }
}
