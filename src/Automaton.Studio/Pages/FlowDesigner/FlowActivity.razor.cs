using AntDesign;
using AntDesign.TableModels;
using Automaton.Studio.Models;
using Automaton.Studio.Pages.Runners;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.FlowDesigner
{
    public partial class FlowActivity : ComponentBase
    {
        [Parameter] public string FlowId { get; set; }
        [Parameter] public string FlowName { get; set; }

        public bool loading;

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private FlowActivityViewModel FlowActivityViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        async Task OnChange(QueryModel queryModel)
        {
            loading = true;

            try
            {
                await FlowActivityViewModel.GetFlowActivity(FlowId, queryModel.StartIndex, queryModel.PageSize);
            }
            catch
            {
                await MessageService.Error(Resources.Errors.FlowsActivityNotLoaded);
            }
            finally
            {
                loading = false;
            }
        }

        private async Task OnRowExpand(RowData<FlowExecution> rowData)
        {
            if (rowData.Expanded)
            {
                rowData.Data.LogsText = await FlowActivityViewModel.GetLogsText(FlowId, rowData.Data.Id);
                StateHasChanged();
            }
        }

        private static bool LogIsLoading(string log)
        {
            return string.IsNullOrEmpty(log);
        }
    }
}
