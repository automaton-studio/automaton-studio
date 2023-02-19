using AutoMapper;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Models;
using Serilog;
using System.Text.Json;

namespace Automaton.Studio.Server.Services;

public class FlowsService
{
    private readonly ApplicationDbContext dataContext;
    private readonly WorkflowExecuteService workflowExecuteService;
    private readonly RunnerService runnerService;
    private readonly IMapper mapper;
    private readonly Guid userId;
    private readonly Serilog.ILogger logger;

    public FlowsService
    (
        ApplicationDbContext dataContext,
        WorkflowExecuteService workflowExecuteService,
        RunnerService runnerService,
        UserContextService userContextService,
        IMapper mapper
    )
    {
        this.dataContext = dataContext;
        this.workflowExecuteService = workflowExecuteService;
        this.runnerService = runnerService;
        this.mapper = mapper;
        this.userId = userContextService.GetUserId();
        this.logger = Log.ForContext<FlowsService>();
    }

    public IEnumerable<FlowInfo> List()
    {
        var entities = dataContext.Flows.Where(x => x.FlowUsers.Any(x => x.UserId == userId));
        var flows = mapper.Map<IEnumerable<FlowInfo>>(entities);

        return flows;
    }

    public Flow Get(Guid id)
    {
        var entity = dataContext.Flows.SingleOrDefault(x => x.Id == id && x.FlowUsers.Any(x => x.UserId == userId));

        var flow = DeserializeFlow(entity.Body);

        return flow;
    }

    public Guid Create(Flow flow)
    {
        flow.Id = Guid.NewGuid();

        var flowEntity = new Entities.Flow
        {
            Id = flow.Id,
            Name = flow.Name,
            Body = JsonSerializer.Serialize(flow),
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };

        dataContext.Flows.Add(flowEntity);

        var flowUser = new Entities.FlowUser
        {
            FlowId = flowEntity.Id,
            UserId = userId
        };

        dataContext.FlowUsers.Add(flowUser);

        dataContext.SaveChanges();

        return flowEntity.Id;
    }

    public void Update(Guid id, Flow flow)
    {
        var entity = dataContext.Flows.SingleOrDefault(x => x.Id == id && x.FlowUsers.Any(x => x.UserId == userId));

        entity.Name = flow.Name;
        entity.Body = JsonSerializer.Serialize(flow);
        entity.Updated = DateTime.UtcNow;

        dataContext.SaveChanges();
    }

    public void Remove(Guid id)
    {
        var flow = dataContext.Flows.SingleOrDefault(x => x.Id == id && x.FlowUsers.Any(x => x.UserId == userId));

        dataContext.Flows.Remove(flow);

        dataContext.SaveChanges();
    }

    public bool Exists(string name)
    {
        var exists = dataContext.Flows.Any(x => x.Name == name && x.FlowUsers.Any(x => x.UserId == userId));

        return exists;
    }

    /// <summary>
    /// Executes flow on server
    /// </summary>
    public async Task<WorkflowExecutorResult> Execute(Guid flowId, CancellationToken cancellationToken)
    {
        var flow = Get(flowId);

        var result = await workflowExecuteService.Execute(flow, cancellationToken);

        return result;
    }

    /// <summary>
    /// Executes flow on runners
    /// </summary>
    public async Task Execute(Guid flowId, IEnumerable<Guid> runnerIds)
    {
        await runnerService.ExecuteFlow(flowId, runnerIds);
    }

    private static Flow DeserializeFlow(string jsonFlow)
    {
        var flow = JsonSerializer.Deserialize<Flow>(jsonFlow);

        return flow;
    }
}
