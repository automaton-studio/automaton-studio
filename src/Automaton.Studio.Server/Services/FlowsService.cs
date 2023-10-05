using AutoMapper;
using Automaton.Core.Enums;
using Automaton.Core.Models;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Models;
using Serilog;
using System.Text.Json;

namespace Automaton.Studio.Server.Services;

public class FlowsService
{
    private readonly ApplicationDbContext dbContext;
    private readonly RunnerService runnerService;
    private readonly ScheduleService scheduleService;
    private readonly IMapper mapper;
    private readonly Guid userId;
    private readonly Serilog.ILogger logger;

    public FlowsService
    (
        ApplicationDbContext dbContext,
        RunnerService runnerService,
        ScheduleService scheduleService,
        UserContextService userContextService,
        IMapper mapper
    )
    {
        this.dbContext = dbContext;
        this.runnerService = runnerService;
        this.scheduleService = scheduleService;
        this.mapper = mapper;
        this.userId = userContextService.GetUserId();
        this.logger = Log.ForContext<FlowsService>();
    }

    public IEnumerable<FlowInfo> List()
    {
        var flows =
            from m in dbContext.Flows
            from s in dbContext.FlowExecutions
                .Where(s => m.Id == s.FlowId)
                .DefaultIfEmpty()
                .OrderByDescending(x => x.Started)
                .Take(1)      
            select new FlowInfo
            {
                Id = m.Id,
                Created = m.Created,
                Updated = m.Updated,
                Name = m.Name,
                Started = s != null ? s.Started : DateTime.MinValue,
                Finished = s != null ? s.Finished : DateTime.MinValue,
                Status = s != null ? s.Status : WorkflowStatus.None.ToString()
            };

        return flows;
    }

    public Flow Get(Guid id)
    {
        var entity = dbContext.Flows.SingleOrDefault(x => x.Id == id && x.FlowUsers.Any(x => x.UserId == userId));

        var flow = DeserializeFlow(entity.Body);

        return flow;
    }

    public Guid Create(Flow flow)
    {
        flow.Id = Guid.NewGuid();
        flow.Created = DateTime.UtcNow;
        flow.Updated = DateTime.UtcNow;

        var flowEntity = new Entities.Flow
        {
            Id = flow.Id,
            Name = flow.Name,
            Body = JsonSerializer.Serialize(flow),
            Created = flow.Created,
            Updated = flow.Updated
        };

        dbContext.Flows.Add(flowEntity);

        var flowUser = new Entities.FlowUser
        {
            FlowId = flowEntity.Id,
            UserId = userId
        };

        dbContext.FlowUsers.Add(flowUser);

        dbContext.SaveChanges();

        return flowEntity.Id;
    }

    public void Update(Guid id, Flow flow)
    {
        var entity = dbContext.Flows.SingleOrDefault(x => x.Id == id && x.FlowUsers.Any(x => x.UserId == userId));

        entity.Name = flow.Name;
        entity.Body = JsonSerializer.Serialize(flow);
        entity.Updated = DateTime.UtcNow;

        dbContext.SaveChanges();
    }

    public void Remove(Guid id)
    {
        var flow = dbContext.Flows.SingleOrDefault(x => x.Id == id && x.FlowUsers.Any(x => x.UserId == userId));

        dbContext.Flows.Remove(flow);

        dbContext.SaveChanges();
    }

    public bool Exists(string name)
    {
        var exists = dbContext.Flows.Any(x => x.Name == name && x.FlowUsers.Any(x => x.UserId == userId));

        return exists;
    }

    public async Task<IEnumerable<RunnerFlowResult>> Execute(Guid flowId, IEnumerable<Guid> runnerIds, CancellationToken cancellationToken)
    {
        var result = await runnerService.ExecuteFlow(flowId, runnerIds, cancellationToken);

        return result;
    }

    private static Flow DeserializeFlow(string jsonFlow)
    {
        var flow = JsonSerializer.Deserialize<Flow>(jsonFlow);

        return flow;
    }
}
