using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Automaton.App.Cron.Components;

public partial class CronJob : ComponentBase
{
    private bool loading = false;

    [Inject] private MessageService MessageService { get; set; }

    private CronJobViewModel CronJobViewModel { get; set; } = new CronJobViewModel();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
    }

    private async Task OnFinish(EditContext editContext)
    {
        try
        {
            loading = true;

            await CronJobViewModel.AddCronJob();

            await MessageService.Info("Schedule added");
        }
        catch (Exception ex)
        {
            await MessageService.Error("Adding schedule failed");
        }
        finally
        {
            loading = false;
        }
    }

    private void OnFinishFailed(EditContext editContext)
    {
        // Do nothing if form validation fails
    }
}
