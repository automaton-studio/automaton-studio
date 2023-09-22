using AutoMapper;
using Automaton.Studio.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Automaton.Studio.Server.Services;

public class LogsService
{
    private readonly ApplicationDbContext dataContext;
    private readonly IMapper mapper;
    private readonly string userName;
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
        this.userName = userContextService.GetUserName();
        this.logger = Serilog.Log.ForContext<LogsService>();
    }

    public IEnumerable<Entities.Log> GetFlowExecutionLogs(Guid executionId)
    {
        try
        {
            var logs = dataContext.Logs.FromSql($"SELECT * FROM `automaton.studio`.`logs` WHERE `username` = {userName} and `properties` -> '$.WorkflowExecutionId' = {executionId}").ToList();

            return logs;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
