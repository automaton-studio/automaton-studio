using Automaton.App.Cron.Models;

namespace Automaton.App.Cron.Components;

public class CronJobViewModel
{
    public CronJobViewModel()
    {
    }

    public async Task AddCronJob()
    {
        var cronJob = new CronJobModel();
    }
}
