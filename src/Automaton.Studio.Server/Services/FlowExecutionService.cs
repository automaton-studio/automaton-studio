﻿using AutoMapper;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Automaton.Studio.Server.Services;

public class FlowExecutionService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMapper mapper;
    private readonly Guid userId;
    private readonly Serilog.ILogger logger;

    public FlowExecutionService
    (
        ApplicationDbContext dbContext,
        UserContextService userContextService,
        IMapper mapper
    )
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.userId = userContextService.GetUserId();
        this.logger = Log.ForContext<FlowsService>();
    }

    public IEnumerable<FlowExecution> List()
    {
        var executions = dbContext.FlowExecutions.Where(x => x.FlowExecutionUsers.Any(x => x.UserId == userId));
        return executions;
    }

    public FlowExecution Get(Guid id)
    {
        var execution = dbContext.FlowExecutions.SingleOrDefault(x => x.Id == id && x.FlowExecutionUsers.Any(x => x.UserId == userId));

        return execution;
    }

    public int Add(Models.FlowExecution executionModel)
    {
        var execution = new FlowExecution()
        {
            Id = executionModel.Id,
            FlowId = executionModel.FlowId,
            Status = executionModel.Status,
            Started = executionModel.Started,
            Finished = executionModel.Finished
        };

        dbContext.FlowExecutions.Add(execution);

        var executionUser = new FlowExecutionUser
        {
            FlowExecutionId = execution.Id,
            UserId = userId
        };

        dbContext.FlowExecutionUsers.Add(executionUser);

        var result = dbContext.SaveChanges();

        return result;
    }

    public async Task Update(Guid id, Models.FlowExecution flowExecution, CancellationToken cancellationToken)
    {
        var entity = dbContext.FlowExecutions
            .Include(x => x.FlowExecutionUsers)
            .SingleOrDefault(x => x.Id == id &&
                x.FlowExecutionUsers.Any(x => x.UserId == userId));

        entity.Status = flowExecution.Status;
        entity.Finished = flowExecution.Finished;

        // Mark entity as modified
        dbContext.Entry(entity).State = EntityState.Modified;

        // Update runner entity
        dbContext.Update(entity);

        // Save changes
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}