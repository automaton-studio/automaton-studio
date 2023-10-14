using AntDesign;
using AntDesign.TableModels;
using Automaton.Studio.Models;
using Automaton.Studio.Pages.Runners;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.FlowDesigner
{
    public partial class FlowSchedule : ComponentBase
    {
        private bool loading;
        private bool newSchedule;
        private Form<FlowScheduleModel> form;

        [Parameter] public string FlowId { get; set; }
        [Parameter] public string FlowName { get; set; }

        [Inject] private NavigationManager NavigationManager { get; set; } = default!;
        [Inject] private FlowScheduleViewModel FlowScheduleViewModel { get; set; } = default!;
        [Inject] private ModalService ModalService { get; set; }
        [Inject] private MessageService MessageService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            FlowScheduleViewModel.FlowId = Guid.Parse(FlowId);

            await base.OnInitializedAsync();
        }

        async Task OnChange(QueryModel queryModel)
        {
            loading = true;

            try
            {
                await FlowScheduleViewModel.GetFlowSchedules(queryModel.StartIndex, queryModel.PageSize);
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

        private Dictionary<string, object> OnRow(RowData<FlowScheduleModel> row)
        {
            if (row.RowIndex == 1 && newSchedule)
            {
                row.Expanded = true;
                newSchedule = false;
            }

            return new Dictionary<string, object>();
        }

        private void NewSchedule()
        {
            newSchedule = true;
            FlowScheduleViewModel.AddNewSchedule();
        }
    }
}
