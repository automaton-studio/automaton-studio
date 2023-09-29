using AntDesign;
using AntDesign.TableModels;
using Automaton.Studio.Models;
using Microsoft.AspNetCore.Components;

namespace Automaton.Studio.Pages.FlowDesigner;

public partial class FlowLogsPage : ComponentBase
{
    [Parameter] public string FlowId { get; set; }
    [Parameter] public string FlowName { get; set; }

    public bool loading;

    [Inject] private FlowLogsViewModel FlowLogsViewModel { get; set; } = default!;
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
            await FlowLogsViewModel.GetLogs(FlowId, queryModel.StartIndex, queryModel.PageSize);
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

    Dictionary<string, object> OnRow(RowData<LogModel> row) => new()
    {
        ["onclick"] = ((Action)delegate
        {
            row.Expanded = !row.Expanded;
        })
    };

    private static bool LogIsLoading(string log)
    {
        return string.IsNullOrEmpty(log);
    }
}
