using AutoMapper;
using Automaton.Studio.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Automaton.Studio.Server.Services;

public class LogsService
{
    private readonly ApplicationDbContext dataContext;
    private readonly IMapper mapper;
    private readonly Guid userId;
    private readonly Serilog.ILogger logger;

    public LogsService
    (
        ApplicationDbContext dataContext,
        UserContextService userContextService,
        IMapper mapper
    )
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
        this.userId = userContextService.GetUserId();
        this.logger = Serilog.Log.ForContext<LogsService>();
    }

    public IEnumerable<Entities.Log> GetFlowExecutionLogs(Guid executionId)
    {
        var logs = dataContext.Logs.FromSql($"SELECT * FROM `automaton.studio`.`logs` WHERE `userid` = {userId} and `properties` -> '$.WorkflowExecutionId' = {executionId}").ToList();

        return logs;
    }
}
