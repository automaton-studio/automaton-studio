using Automaton.Studio.Activities;
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

        public void OnItemDrop(DynamicActivity item)
        {
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            DesignerViewModel.ActiveItemChanged += OnActiveItemChanged;

            if (!string.IsNullOrEmpty(WorkflowId))
            {
                await DesignerViewModel.LoadWorkflow(WorkflowId);
            }
        }

        private void OnActiveItemChanged(object? sender, ActivityChangedEventArgs e)
        {
            dropzone.ActiveItem = e.Activity;
        }
    }
}
