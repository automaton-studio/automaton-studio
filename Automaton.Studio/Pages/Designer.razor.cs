using Automaton.Activity;
using Automaton.Studio.Events;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Plk.Blazor.DragDrop;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;

        [Parameter] public string WorkflowId { get; set; }

        private Dropzone<DynamicActivity>? dropzone;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            DesignerViewModel.ActivityChanged += OnActivityChanged;

            if (!string.IsNullOrEmpty(WorkflowId))
            {
                await DesignerViewModel.LoadWorkflow(WorkflowId);
            }
        }

        private void OnActivityChanged(object? sender, ActivityChangedEventArgs e)
        {
            dropzone.ActiveItem = e.Activity;
        }

        private void OnItemDrop(DynamicActivity item)
        {
        }
    }
}
