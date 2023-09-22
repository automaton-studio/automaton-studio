using AntDesign;
using AntDesign.TableModels;
using Automaton.Studio.Models;
using Automaton.Studio.Pages.Runners;
using Microsoft.AspNetCore.Components;
using static IronPython.Runtime.Profiler;

namespace Automaton.Studio.Pages.FlowDesigner
{
    public partial class FlowActivity : ComponentBase
    {
        [Parameter] public string FlowId { get; set; }

        public bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private FlowActivityViewModel FlowActivityViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            loading = true;

            try
            {
                await FlowActivityViewModel.GetFlowActivity(FlowId);
            }
            catch
            {
                await MessageService.Error(Resources.Errors.FlowsActivityNotLoaded);
            }
            finally
            {
                loading = false;
            }

            await base.OnInitializedAsync();
        }

        private async Task OnRowExpand(RowData<FlowExecution> rowData)
        {
            rowData.Data.LogsText = await FlowActivityViewModel.GetLogsctivity(FlowId, rowData.Data.Id);

            StateHasChanged();
        }

        private static bool LogIsLoading(string log)
        {
            return string.IsNullOrEmpty(log);
        }
    }
}
