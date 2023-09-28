using AutoMapper;
using Automaton.Studio.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Automaton.Studio.Server.Services;

public class FlowLogsService
{
    private readonly ApplicationDbContext dbContext;
    private readonly IMapper mapper;
    private readonly string userName;
    private readonly Serilog.ILogger logger;

    public FlowLogsService
    (
        ApplicationDbContext dbContext,
        UserContextService userContextService,
        IMapper mapper
    )
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.userName = userContextService.GetUserName();
        this.logger = Serilog.Log.ForContext<FlowLogsService>();
    }

    public async Task<IEnumerable<Entities.Log>> GetListAsync(Guid flowId, int startIndex, int pageSize)
    {
        var log = await dbContext.Logs.FromSql($"SELECT * FROM Logs WHERE UserName = {userName} and Properties -> '$.WorkflowId' = {flowId}")
            .OrderByDescending(x => x.Timestamp)
            .Skip(startIndex)
            .Take(pageSize)
            .ToListAsync();

        return log;
    }

    public async Task<int> GetTotalAsync(Guid flowId)
    {
        var total = await dbContext.Logs.FromSql($"SELECT * FROM Logs WHERE UserName = {userName} and Properties -> '$.WorkflowId' = {flowId}").CountAsync();

        return total;
    }

    public IEnumerable<Entities.Log> GetFlowExecutionLogs(Guid executionId)
    {
        try
        {
            var logs = dbContext.Logs.FromSql($"SELECT * FROM Logs WHERE UserName = {userName} and Properties -> '$.WorkflowExecutionId' = {executionId}").ToList();

            return logs;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
