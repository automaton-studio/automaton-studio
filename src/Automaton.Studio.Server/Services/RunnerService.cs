using AutoMapper;
using Automaton.Core.Enums;
using Automaton.Core.Models;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Automaton.Studio.Server.Models;
using Azure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using Serilog;
using System.Threading;

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

    public async Task<IEnumerable<RunnerFlowResult>> ExecuteFlow(Guid flowId, IEnumerable<Guid> runnerIds, CancellationToken cancellationToken)
    {
        var results = new List<RunnerFlowResult>();

        foreach (var runnerId in runnerIds)
        {
            RunnerFlowResult result;
            var runner = Get(runnerId);
            var client = automatonHub.Clients.Client(runner.ConnectionId);

            try
            {
                var response = await client.InvokeAsync<WorkflowExecution>(AutomatonHubMethods.RunWorkflow, flowId, cancellationToken);

                result = GetSuccessfulFlowResult(flowId: flowId, runnerId: runnerId, response);
            }
            catch (Exception ex)
            {
                logger.ForContext("FlowId", flowId)
                      .ForContext("RunnerId", runnerId)
                      .Error(ex, "An error happened when executing flow on runner");

                result = GetInvalidFlowResult(flowId: flowId, runnerId: runnerId);
            }

            results.Add(result);
        }

        return results;
    }

    private async Task<IEnumerable<RunnerDetails>> GetRunnersWithUpdatedStatus(IEnumerable<Runner> runnerEntities, CancellationToken cancellationToken)
    {
        var runners = mapper.Map<IEnumerable<RunnerDetails>>(runnerEntities);

        foreach (var runner in runners)
        {
            try
            {
                var client = automatonHub.Clients.Client(runner.ConnectionId);

                var response = await client.InvokeAsync<string>(AutomatonHubMethods.Ping, cancellationToken);

                runner.Status = GetRunnerStatus(response);
            }
            catch (Exception ex)
            {
                runner.Status = RunnerStatus.Offline;

                logger.ForContext("RunnerName", runner.Name)
                      .ForContext("RunnerId", runner.Id)
                      .Error(ex, "Could not get runner status");
            }
        }

        return runners;
    }

    private static RunnerStatus GetRunnerStatus(string response)
    {
        return response.Equals("Pong", StringComparison.OrdinalIgnoreCase) ? 
            RunnerStatus.Online : 
            RunnerStatus.Offline;
    }

    private RunnerFlowResult GetSuccessfulFlowResult(Guid flowId, Guid runnerId, WorkflowExecution flowExecution)
    {
        return new RunnerFlowResult
        {
            FlowId = flowId,
            RunnerId = runnerId,
            Started = flowExecution.Started,
            Finished = flowExecution.Finished,
            Status = flowExecution.Status
        };
    }

    private RunnerFlowResult GetInvalidFlowResult(Guid flowId, Guid runnerId)
    {
        return new RunnerFlowResult
        {
            FlowId = flowId,
            RunnerId = runnerId,
            Started = DateTime.MinValue,
            Finished = DateTime.MinValue,
            Status = WorkflowStatus.None
        };
    }
}
