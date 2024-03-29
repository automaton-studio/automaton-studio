﻿using AutoMapper;
using Automaton.Core.Enums;
using Automaton.Core.Models;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Automaton.Studio.Server.Models;
using Hangfire;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Automaton.Studio.Server.Services;

public class ScheduleService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IHubContext<AutomatonHub> automatonHub;
    private readonly IMapper mapper;
    private readonly Guid userId;
    private readonly Serilog.ILogger logger;

    public ScheduleService
    (
        ApplicationDbContext dbContext,
        UserContextService userContextService,
        IMapper mapper,
        IHubContext<AutomatonHub> automatonHub
    )
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.automatonHub = automatonHub;
        this.userId = userContextService.GetUserId();
        logger = Serilog.Log.ForContext<ScheduleService>();
    }
        
    public async Task<IEnumerable<ScheduleModel>> ListAsync(Guid flowId, CancellationToken cancellationToken)
    {
        var schedules = await dbContext.Schedules
            .Where(x => x.FlowId == flowId && x.ScheduleUsers
            .Any(x => x.UserId == userId)).ToListAsync(cancellationToken: cancellationToken);

        using var connection = JobStorage.Current.GetConnection();

        var scheduleModels = new List<ScheduleModel>();
        
        foreach (var schedule in schedules)
        {
            var scheduleHash = connection.GetAllEntriesFromHash($"recurring-job:{schedule.Id}");

            var scheduleModel = new ScheduleModel
            {
                Id = schedule.Id,
                Name = schedule.Name,
                Cron = scheduleHash["Cron"],
                CreatedAt = DateTime.Parse(scheduleHash["CreatedAt"]),
                RunnerIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(schedule.RunnerIds),
                CronRecurrence = JsonSerializer.Deserialize<CronRecurrence>(schedule.CronRecurrence)
            };

            scheduleModels.Add(scheduleModel);     
        }

        return scheduleModels;
    }

    public async Task<ScheduleModel> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var schedule = await dbContext.Schedules
            .SingleOrDefaultAsync(x => x.Id == id && x.ScheduleUsers.Any(x => x.UserId == userId), cancellationToken: cancellationToken);

        using var connection = JobStorage.Current.GetConnection();

        var scheduleHash = connection.GetAllEntriesFromHash($"recurring-job:{schedule.Id}");
        var scheduleModel = new ScheduleModel
        {
            Id = schedule.Id,
            Name = schedule.Name,
            FlowId = schedule.FlowId,
            Cron = scheduleHash["Cron"],
            CreatedAt = DateTime.Parse(scheduleHash["CreatedAt"]),
            RunnerIds = JsonSerializer.Deserialize<IEnumerable<Guid>>(schedule.RunnerIds),
            CronRecurrence = JsonSerializer.Deserialize<CronRecurrence>(schedule.CronRecurrence)
        };        

        return scheduleModel;
    }

    public async Task<int> AddAsync(ScheduleModel scheduleModel, CancellationToken cancellationToken)
    {
        var schedule = new Schedule()
        {
            Id = scheduleModel.Id,
            Name = scheduleModel.Name,
            FlowId = scheduleModel.FlowId,
            RunnerIds = JsonSerializer.Serialize(scheduleModel.RunnerIds),
            CronRecurrence = JsonSerializer.Serialize(scheduleModel.CronRecurrence),
        };

        dbContext.Schedules.Add(schedule);

        var scheduleUser = new ScheduleUser
        {
            ScheduleId = schedule.Id,
            UserId = userId
        };

        dbContext.ScheduleUsers.Add(scheduleUser);

        var result = await dbContext.SaveChangesAsync(cancellationToken);

        RecurringJob.AddOrUpdate(schedule.Id.ToString(), 
            () => ExecuteFlow(schedule.FlowId, scheduleModel.RunnerIds, userId, cancellationToken),
            scheduleModel.GetCron());

        return result;
    }

    public async Task UpdateAsync(Guid id, ScheduleModel scheduleModel, CancellationToken cancellationToken)
    {
        var schedule = dbContext.Schedules
            .Include(x => x.ScheduleUsers)
            .SingleOrDefault(x => x.Id == id && x.ScheduleUsers.Any(x => x.UserId == userId));

        schedule.Name = scheduleModel.Name;
        schedule.CronRecurrence = JsonSerializer.Serialize(scheduleModel.CronRecurrence);
        schedule.RunnerIds = JsonSerializer.Serialize(scheduleModel.RunnerIds);

        dbContext.Entry(schedule).State = EntityState.Modified;

        dbContext.Update(schedule);

        await dbContext.SaveChangesAsync(cancellationToken);

        RecurringJob.AddOrUpdate(schedule.Id.ToString(),
            () => ExecuteFlow(schedule.FlowId, scheduleModel.RunnerIds, userId, cancellationToken),
            scheduleModel.GetCron());
    }

    public void Remove(Guid id)
    {
        var schedule = dbContext.Schedules.SingleOrDefault(x => x.Id == id && x.ScheduleUsers.Any(x => x.UserId == userId));

        dbContext.Schedules.Remove(schedule);

        dbContext.SaveChanges();
      
        RecurringJob.RemoveIfExists(schedule.Id.ToString());
    }

    public async Task<IEnumerable<RunnerFlowResult>> ExecuteFlow(Guid flowId, IEnumerable<Guid> runnerIds, Guid userId, CancellationToken cancellationToken)
    {
        var results = new List<RunnerFlowResult>();

        foreach (var runnerId in runnerIds)
        {
            RunnerFlowResult result;
            var runner = GetRunner(runnerId, userId);
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

    private Runner GetRunner(Guid id, Guid userId)
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

    private static RunnerFlowResult GetSuccessfulFlowResult(Guid flowId, Guid runnerId, WorkflowExecution flowExecution)
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

    private static RunnerFlowResult GetInvalidFlowResult(Guid flowId, Guid runnerId)
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
