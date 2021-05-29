using AntDesign;
using Automaton.Studio.Activity;
using Automaton.Studio.Events;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Plk.Blazor.DragDrop;
using System.Reflection;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    public static class ExtensionMethods
    {
        public static async Task<object> InvokeAsync(this MethodInfo @this, object obj, params object[] parameters)
        {
            dynamic awaitable = @this.Invoke(obj, parameters);
            await awaitable;
            return awaitable.GetAwaiter().GetResult();
        }
    }

    partial class Designer : ComponentBase
    {
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;
        [Inject] ModalService ModalService { get; set; } = default!;

        [Parameter] public string WorkflowId { get; set; }

        private Dropzone<StudioActivity>? dropzone;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            DesignerViewModel.DragActivityChanged += OnActivityChanged;

            if (!string.IsNullOrEmpty(WorkflowId))
            {
                await DesignerViewModel.LoadWorkflow(WorkflowId);
            }
        }

        private void OnActivityChanged(object? sender, DragActivityChangedEventArgs e)
        {
            dropzone.ActiveItem = e.Activity;
        }

        private async void OnItemDrop(StudioActivity item)
        {
            await NewActivityDialog(item);
        }

        private async Task NewActivityDialog(StudioActivity activity)
        {
            var modalConfig = new ModalOptions
            {
                Title = activity.DisplayName
            };

            var method = typeof(ModalService).GetMethod(nameof(ModalService.CreateAutomatonModalAsync));
            var generic = method.MakeGenericMethod(activity.GetPropertiesComponent(), activity.GetType());
            var result = await generic.InvokeAsync(ModalService, new object[] { modalConfig, activity }) as ModalRef;

            result.OnOk = () => { return Task.CompletedTask; };
        }
    }
}
