using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Automaton.Studio.Server.Hubs;

[Authorize]
public class AutomatonHub : Hub
{
    private const string UserIdClaim = "uid";
    private const string RunnerIdHeader = "RunnerId";
    private const string RunnerNameHeader = "RunnerName";

    private readonly RunnerService runnerService;

    public AutomatonHub(RunnerService runnerService)
    {
        this.runnerService = runnerService;
    }

    public override async Task OnConnectedAsync()
    {
        var runner = GetRunner();

        await Clients.Caller.SendAsync("WelcomeRunner", $"Welcome {runner.Name}");

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

    private Models.Runner GetRunner()
    {
        var httpContext = Context.GetHttpContext();
        var runnerId = httpContext.Request.Headers[RunnerIdHeader].ToString();
        var runnerName = httpContext.Request.Headers[RunnerNameHeader].ToString();
        var connectionId = Context.ConnectionId;

        var runner = new Models.Runner
        { 
            Id = Guid.Parse(runnerId),
            Name = runnerName,
            ConnectionId = connectionId
        };

        return runner;
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
        var runner = GetRunner();

        await runnerService.Update(runner.Id, runner, CancellationToken.None);
    }
}
