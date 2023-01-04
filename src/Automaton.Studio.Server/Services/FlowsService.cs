using AutoMapper;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Middleware;
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
        var entityFlows = from flow in dataContext.Flows
            join flowUser in dataContext.FlowUsers
            on flow.Id equals flowUser.FlowId
            where flowUser.UserId == userId
            select flow;

        var flows = mapper.Map<IEnumerable<FlowInfo>>(entityFlows);

        return flows;
    }

    public Flow Get(Guid id)
    {
        var flow = 
        (
            from _flow in dataContext.Flows
            join _flowUser in dataContext.FlowUsers
            on _flow.Id equals _flowUser.FlowId
            where _flow.Id == id && _flowUser.UserId == userId
            select DeserializeFlow(_flow.Body)
        )
        .SingleOrDefault();

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
        var entity =
        (
            from _flow in dataContext.Flows
            join _flowUser in dataContext.FlowUsers
            on _flow.Id equals _flowUser.FlowId
            where _flow.Id == id && _flowUser.UserId == userId
            select _flow
        )
        .SingleOrDefault();

        entity.Name = flow.Name;
        entity.Body = JsonSerializer.Serialize(flow);
        entity.Updated = DateTime.UtcNow;

        dataContext.SaveChanges();
    }

    public void Remove(Guid id)
    {
        var flow =
        (
            from _flow in dataContext.Flows
            join _flowUser in dataContext.FlowUsers
            on _flow.Id equals _flowUser.FlowId
            where _flow.Id == id && _flowUser.UserId == userId
            select _flow
        )
        .SingleOrDefault();

        dataContext.Flows.Remove(flow);

        dataContext.SaveChanges();
    }

    public bool Exists(string name)
    {
        var flows =
        (
            from _flow in dataContext.Flows
            join _flowUser in dataContext.FlowUsers
            on _flow.Id equals _flowUser.FlowId
            where _flow.Name == name && _flowUser.UserId == userId
            select _flow
        );

        return flows.Any();
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
    public async Task Execute(Guid flowId, IEnumerable<Guid> runnerIds, CancellationToken cancellationToken)
    {
        await runnerService.ExecuteFlow(flowId, runnerIds, cancellationToken);
    }

    private static Flow DeserializeFlow(string jsonFlow)
    {
        var flow = JsonSerializer.Deserialize<Flow>(jsonFlow);

        return flow;
    }
}
