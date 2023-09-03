using AutoMapper;
using Automaton.Core.Enums;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Automaton.Studio.Server.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Automaton.Studio.Server.Services;

public class RunnerService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IHubContext<AutomatonHub> automatonHub;
    private readonly Guid userId;
    private readonly IMapper mapper;
    private readonly Serilog.ILogger logger;

    public RunnerService(ApplicationDbContext dbContext,
        UserContextService userContextService,
        IMapper mapper,
        IHubContext<AutomatonHub> automatonHub)
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.automatonHub = automatonHub;
        this.userId = userContextService.GetUserId();
        logger = Log.ForContext<RunnerService>();
    }

    public async Task<IEnumerable<RunnerDetails>> List(CancellationToken cancellationToken)
    {
        var runners = dbContext.Runners.Where(x => x.RunnerUsers.Any(x => x.UserId == userId));

        return await GetRunnersWithUpdatedStatus(runners, cancellationToken);
    }

    public async Task<IEnumerable<RunnerDetails>> List(IEnumerable<Guid> runnerIds, CancellationToken cancellationToken)
    {
        var runners = dbContext.Runners.Where(x => x.RunnerUsers.Any(x => x.UserId == userId) && runnerIds.Contains(x.Id));

        return await GetRunnersWithUpdatedStatus(runners, cancellationToken);
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

    public int Add(RunnerDetails runnerModel)
    {
        var runner = new Runner()
        {
            Id = runnerModel.Id,
            Name = runnerModel.Name
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

    public async Task UpdateConnection(Guid runnerId, string connectionId, CancellationToken cancellationToken)
    {
        var runner = dbContext.Runners
            .Include(x => x.RunnerUsers)
            .SingleOrDefault(x => x.Id == runnerId && x.RunnerUsers.Any(x => x.UserId == userId));

        runner.ConnectionId = connectionId;

        // Mark entity as modified
        dbContext.Entry(runner).State = EntityState.Modified;

        // Update runner entity
        dbContext.Update(runner);

        // Save changes
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(Guid id, RunnerDetails runner, CancellationToken cancellationToken)
    {
        var entity = dbContext.Runners
            .Include(x => x.RunnerUsers)
            .SingleOrDefault(x => x.Id == id && x.RunnerUsers.Any(x => x.UserId == userId));

        entity.Name = runner.Name;

        // Mark entity as modified
        dbContext.Entry(entity).State = EntityState.Modified;

        // Update runner entity
        dbContext.Update(entity);

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

    private async Task<IEnumerable<RunnerDetails>> GetRunnersWithUpdatedStatus(IEnumerable<Runner> runnerEntities, CancellationToken cancellationToken)
    {
        var runners = mapper.Map<IEnumerable<RunnerDetails>>(runnerEntities);

        foreach (var runner in runners)
        {
            try
            {
                var client = automatonHub.Clients.Client(runner.ConnectionId);

                var message = await client.InvokeAsync<string>(AutomatonHubMethods.Ping, cancellationToken);

                runner.Status = string.IsNullOrEmpty(message) ? RunnerStatus.Offline : RunnerStatus.Online;
            }
            catch(Exception ex)
            {
                runner.Status = RunnerStatus.Offline;
                logger.Warning("Runner {0} is offline", runner.Name);
            }
        }

        return runners;
    }
}
