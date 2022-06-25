using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Automaton.Studio.Server.Hubs;

[Authorize]
public class AutomatonHub : Hub
{
    private const string UserIdClaim = "uid";
    private const string RunnerNameHeader = "RunnerName";

    private readonly RunnerService runnerService;

    public AutomatonHub(RunnerService runnerService)
    {
        this.runnerService = runnerService;
    }

    public override async Task OnConnectedAsync()
    {
        var runnerName = GetRunnerName();

        await Clients.Caller.SendAsync("WelcomeRunner", $"Welcome {runnerName}");

        await UpdateRunnerConnection();

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }

    public bool Ping(string runnerName)
    {
        return !string.IsNullOrEmpty(runnerName);
    }

    private string GetRunnerName()
    {
        var httpCtx = Context.GetHttpContext();
        var runnerName = httpCtx.Request.Headers[RunnerNameHeader].ToString();

        return runnerName;
    }

    private string GetUserId()
    {
        var userIdClaim = Context.User.Claims.SingleOrDefault(x => x.Type == UserIdClaim);

        if (userIdClaim is null)
            throw new ArgumentNullException("userId");

        return userIdClaim.Value;
    }

    private async Task UpdateRunnerConnection()
    {
        var runner = new Models.Runner
        {
            Name = GetRunnerName(),
            ConnectionId = Context.ConnectionId
        };

        await runnerService.Update(runner, CancellationToken.None);
    }
}
