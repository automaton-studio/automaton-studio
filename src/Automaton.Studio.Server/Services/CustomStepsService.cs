using AutoMapper;
using Automaton.Core.Models;
using Automaton.Core.Services;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Models;
using Serilog;
using System.Text.Json;

namespace Automaton.Studio.Server.Services;

public class CustomStepsService
{
    private readonly ApplicationDbContext dataContext;
    private readonly WorkflowExecuteService workflowExecuteService;
    private readonly IMapper mapper;
    private readonly Guid userId;
    private readonly Serilog.ILogger logger;

    public CustomStepsService
    (
        ApplicationDbContext dataContext,
        WorkflowExecuteService workflowExecuteService,
        UserContextService userContextService,
        IMapper mapper
    )
    {
        this.dataContext = dataContext;
        this.workflowExecuteService = workflowExecuteService;
        this.mapper = mapper;
        this.userId = userContextService.GetUserId();
        this.logger = Log.ForContext<FlowsService>();
    }

    public IEnumerable<CustomStep> List()
    {
        var entities = dataContext.CustomSteps.Where(x => x.CustomStepUsers.Any(x => x.UserId == userId));
        var steps = mapper.Map<IEnumerable<CustomStep>>(entities);

        return steps;
    }

    public CustomStep Get(Guid id)
    {
        var stepEntity = dataContext.CustomSteps.SingleOrDefault(x => x.Id == id && x.CustomStepUsers.Any(x => x.UserId == userId));
        var step = mapper.Map<CustomStep>(stepEntity);
 
        return step;
    }

    public Guid Create(CustomStep step)
    {
        step.Id = Guid.NewGuid();

        var entity = new Entities.CustomStep
        {
            Id = step.Id,
            Name = step.Name,
            DisplayName = step.DisplayName,
            Description = step.Description,
            Icon = step.Icon,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Definition = JsonSerializer.Serialize(step.Definition)
        };

        dataContext.CustomSteps.Add(entity);

        var stepUser = new Entities.CustomStepUser
        {
            CustomStepId = entity.Id,
            UserId = userId
        };

        dataContext.CustomStepUsers.Add(stepUser);

        dataContext.SaveChanges();

        return entity.Id;
    }

    public void Update(Guid id, CustomStep step)
    {
        var entity = dataContext.CustomSteps.SingleOrDefault(x => x.Id == id && x.CustomStepUsers.Any(x => x.UserId == userId));

        entity.Name = step.Name;
        entity.DisplayName = step.DisplayName;
        entity.Description = step.Description;
        entity.Icon = step.Icon;
        entity.Updated = DateTime.UtcNow;
        entity.Definition = JsonSerializer.Serialize(step.Definition);

        dataContext.SaveChanges();
    }

    public void Remove(Guid id)
    {
        var entity = dataContext.CustomSteps.SingleOrDefault(x => x.Id == id && x.CustomStepUsers.Any(x => x.UserId == userId));

        dataContext.CustomSteps.Remove(entity);

        dataContext.SaveChanges();
    }

    public bool Exists(string name)
    {
        var exists = dataContext.CustomSteps.Any(x => x.Name == name && x.CustomStepUsers.Any(x => x.UserId == userId));

        return exists;
    }
}
