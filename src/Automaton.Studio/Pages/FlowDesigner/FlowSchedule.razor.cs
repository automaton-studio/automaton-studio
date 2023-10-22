using AntDesign;
using AntDesign.TableModels;
using Automaton.Studio.Enums;
using Automaton.Studio.Models;
using Automaton.Studio.Pages.Runners;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Automaton.Studio.Pages.FlowDesigner;

public partial class FlowSchedule : ComponentBase
{
    private bool loading;
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

        await FlowScheduleViewModel.GetRunners();

        await base.OnInitializedAsync();
    }

    private async Task OnChange(QueryModel queryModel)
    {
        try
        {
            loading = true;

            await FlowScheduleViewModel.GetFlowSchedules(queryModel.StartIndex, queryModel.PageSize);
        }
        catch
        {
            await MessageService.Error("Schedules not loaded");
        }
        finally
        {
            loading = false;
        }
    }

    private async Task Save(EditContext editContext)
    {
        var schedule = editContext.Model as FlowScheduleModel;

        try
        {
            schedule.Loading = true;

            if (schedule.IsNew)
            {
                await FlowScheduleViewModel.AddSchedule(schedule);
                await MessageService.Info("Schedule created");
            }
            else
            {
                await FlowScheduleViewModel.UpdateSchedule(schedule);
                await MessageService.Info("Schedule updated");
            }
        }
        catch (Exception ex)
        {
            await MessageService.Error("Schedule update failed");
        }
        finally
        {
            schedule.Loading = false;
        }
    }

    private async Task DeleteSchedule(Guid id)
    {
        try
        {
            loading = true;

            await FlowScheduleViewModel.DeleteSchedule(id);

            await MessageService.Info("Schedule deleted");

            StateHasChanged();
        }
        catch (Exception ex)
        {
            await MessageService.Error("Schedule delete failed");
        }
        finally
        {
            loading = false;
        }
    }

    private void SaveFailed(EditContext editContext)
    {
        // Do nothing if form validation fails
    }

    private Dictionary<string, object> OnRow(RowData<FlowScheduleModel> row)
    {
        if (row.RowIndex == 1 && row.Data.IsNew)
        {
            row.Expanded = true;
        }

        return new Dictionary<string, object>();
    }

    private void NewSchedule()
    {
        FlowScheduleViewModel.NewSchedule();
    }

    private void OnCronTypeChange(CronType cronType)
    {
    }

    private bool IsMinuteHidden(CronType cronType)
    {
        return cronType == CronType.Minutely;
    }

    private bool IsHourHidden(CronType cronType)
    {
        return cronType == CronType.Minutely ||
            cronType == CronType.Hourly;
    }

    private bool IsDayHidden(CronType cronType)
    {
        return cronType == CronType.Minutely ||
            cronType == CronType.Hourly ||
            cronType == CronType.Daily ||
            cronType == CronType.Weekly;
    }

    private bool IsDayOfWeekHidden(CronType cronType)
    {
        return cronType == CronType.Minutely ||
           cronType == CronType.Hourly ||
           cronType == CronType.Daily ||
           cronType == CronType.Monthly ||
           cronType == CronType.Yearly;
    }

    private bool IsMonthHidden(CronType cronType)
    {
        return cronType == CronType.Minutely || 
            cronType == CronType.Hourly || 
            cronType == CronType.Daily || 
            cronType == CronType.Weekly ||
            cronType == CronType.Monthly;
    }
}
