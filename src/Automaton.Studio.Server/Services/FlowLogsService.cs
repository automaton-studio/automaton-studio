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
    private readonly ConfigurationService configurationService;

    public FlowLogsService
    (
        ApplicationDbContext dbContext,
        UserContextService userContextService,
        ConfigurationService configurationService,
        IMapper mapper
    )
    {
        this.dbContext = dbContext;
        this.mapper = mapper;
        this.configurationService = configurationService;
        this.userName = userContextService.GetUserName();
        this.logger = Serilog.Log.ForContext<FlowLogsService>();
    }

    public async Task<IEnumerable<Entities.Log>> GetListAsync(Guid flowId, int startIndex, int pageSize)
    {
        // TODO! Add MsSql implementation
        if (configurationService.IsDatabaseTypeMsSql())
        {
            return Enumerable.Empty<Entities.Log>();
        }

        var log = await dbContext.Logs.FromSql($"SELECT * FROM Logs WHERE UserName = {userName} and Properties -> '$.WorkflowId' = {flowId}")
            .OrderByDescending(x => x.Timestamp)
            .Skip(startIndex)
            .Take(pageSize)
            .ToListAsync();

        return log;
    }

    public async Task<int> GetTotalAsync(Guid flowId)
    {
        // TODO! Add MsSql implementation
        if (configurationService.IsDatabaseTypeMsSql())
        {
            return 0;
        }

        var total = await dbContext.Logs.FromSql($"SELECT * FROM Logs WHERE UserName = {userName} and Properties -> '$.WorkflowId' = {flowId}").CountAsync();

        return total;
    }

    public IEnumerable<Entities.Log> GetFlowExecutionLogs(Guid executionId)
    {
        // TODO! Add MsSql implementation
        if (configurationService.IsDatabaseTypeMsSql())
        {
            return Enumerable.Empty<Entities.Log>();
        }

        var logs = dbContext.Logs.FromSql($"SELECT * FROM Logs WHERE UserName = {userName} and Properties -> '$.WorkflowExecutionId' = {executionId}").ToList();

        return logs;
    }
}
