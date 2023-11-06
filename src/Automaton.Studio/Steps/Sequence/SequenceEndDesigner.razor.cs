using AntDesign;
using Automaton.Studio.Domain;
using Automaton.Studio.Events;
using Automaton.Studio.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Automaton.Studio.Steps.Sequence;

public partial class SequenceEndDesigner : ComponentBase
{
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private ModalService ModalService { get; set; } = default!;

    [Parameter] public SequenceEndStep Step { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
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

    private async Task OnKeyDown(KeyboardEventArgs e, StudioStep step)
    {
        if (e.Key == "Delete")
        {
            await OnDelete(step);
        }
    }
}
