using AntDesign;
using Automaton.Studio.Activity;
using Automaton.Studio.Events;
using Automaton.Studio.Extensions;
using Automaton.Studio.ViewModels;
using Microsoft.AspNetCore.Components;
using Plk.Blazor.DragDrop;
using System.Threading.Tasks;

namespace Automaton.Studio.Pages
{
    partial class Designer : ComponentBase
    {
        #region Members

        private Dropzone<StudioActivity>? dropzone;

        #endregion

        #region DI

        [Inject] ModalService ModalService { get; set; } = default!;
        [Inject] private IDesignerViewModel DesignerViewModel { get; set; } = default!;

        #endregion

        #region Params

        [Parameter] public string WorkflowId { get; set; }

        #endregion

        #region Overrides

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            DesignerViewModel.DragActivityChanged += OnActivityChanged;

            if (!string.IsNullOrEmpty(WorkflowId))
            {
                await DesignerViewModel.LoadWorkflow(WorkflowId);
            }
        }

        #endregion

        #region Private Methods

        private void OnActivityChanged(object? sender, DragActivityChangedEventArgs e)
        {
            dropzone.ActiveItem = e.Activity;
        }

        private async void CreateActivity(StudioActivity activity)
        {
            var modalConfig = new ModalOptions
            {
                Title = activity.DisplayName
            };

            var method = typeof(ModalService).GetMethod(nameof(ModalService.CreateDynamicModalAsync));
            var generic = method.MakeGenericMethod(activity.GetPropertiesComponent(), activity.GetType());
            var result = await generic.InvokeAsync(ModalService, new object[] { modalConfig, activity }) as ModalRef;

            result.OnOk = () => { return Task.CompletedTask; };
        }

        #endregion
    }
}
