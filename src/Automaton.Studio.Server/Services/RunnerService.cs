using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Automaton.Studio.Server.Services;

public class RunnerService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IHubContext<AutomatonHub> automatonHub;
    private readonly Guid userId;

    public RunnerService(ApplicationDbContext dbContext,
        UserContextService userContextService,
        IHubContext<AutomatonHub> automatonHub)
    {
        this.dbContext = dbContext;
        this.automatonHub = automatonHub;
        this.userId = userContextService.GetUserId();
    }

    public IEnumerable<Runner> List()
    {
        var runners = dbContext.Runners.Where(x => x.RunnerUsers.Any(x => x.UserId == userId));
        return runners;
    }

    public IEnumerable<Runner> List(IEnumerable<Guid> runnerIds)
    {
        var runners = dbContext.Runners.Where(x => x.RunnerUsers.Any(x => x.UserId == userId) && runnerIds.Contains(x.Id));

        return runners;
    }

    public Runner Get(Guid id)
    {
        var runner = dbContext.Runners.SingleOrDefault(x => x.Id == id && x.RunnerUsers.Any(x => x.UserId == userId));

        // Because we update Runner's ConnectionId on the fly,
        // when retrieving data we get the cached version of it
        // with previous ConnectionId. There is no need to do the
        // same thing with other entities if they aren't updated
        // in the same way as the Runner entity.

        // Here are some ideas to fix the issue:
        // https://stackoverflow.com/a/51290890/778863
        // http://codethug.com/2016/02/19/Entity-Framework-Cache-Busting/

        // Solution 1. Reload the entity 
        dbContext.Entry(runner).Reload();

        // Solution 2. Detach the entity to remove it from context’s cache.
        // dbContext.Entry(entity).State = EntityState.Detached;
        // entity = dbContext.Runners.Find(id);

        return runner;
    }

    public int Create(Models.RegisterRunnerDetails runnerDetails)
    {
        var runner = new Runner()
        {
            Name = runnerDetails.Name
        };

        dbContext.Runners.Add(runner);

        var runnerUser = new RunnerUser
        {
            RunnerId = runner.Id,
            UserId = userId
        };

        dbContext.RunnerUsers.Add(runnerUser);

        var result = dbContext.SaveChanges();

        return result;
    }

    public async Task Update(Models.Runner runner, CancellationToken cancellationToken)
    {
        var runnerEntity = dbContext.Runners
            .Include(x => x.RunnerUsers)
            .SingleOrDefault(x => x.Name == runner.Name && 
                x.RunnerUsers.Any(x => x.UserId == userId));

        if (runnerEntity == null)
        {
            throw new ArgumentException("Runner not found");
        }

        // Update connection id
        runnerEntity.ConnectionId = runner.ConnectionId;

        // Mark entity as modified
        dbContext.Entry(runnerEntity).State = EntityState.Modified;

        // Update runner entity
        dbContext.Update(runnerEntity);

        // Save changes
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public bool Exists(string name)
    {
        var exists = dbContext.Runners.Any(x =>
            x.Name.ToLower() == name.ToLower() &&
            x.RunnerUsers.Any(x => x.UserId == userId));

        return exists;
    }

    public async Task ExecuteFlow(Guid flowId, IEnumerable<Guid> runnerIds)
    {
        foreach (var runnerId in runnerIds)
        {
            var runner = Get(runnerId);
            var client = automatonHub.Clients.Client(runner.ConnectionId);

            await client.SendAsync(AutomatonHubMethods.RunWorkflow, flowId);
        }
    }
}
