using AutoMapper;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection.Metadata;
using System.Text.Json;

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

    //public IEnumerable<Entities.Log> List()
    //{
    //    var entities = dataContext.Logs;
    //    //var steps = mapper.Map<IEnumerable<LogEvent>>(entities);

    //    return entities;
    //}

    //public IEnumerable<Entities.Log> GetFlowExecutionLogs(Guid executionId)
    //{
    //    //var logs = dataContext.Logs.FromSql<Entities.Log>($"SELECT * FROM `automaton.studio`.`logs` WHERE `properties` -> '$.WorkflowExecutionId' = {executionId}").ToList();

    //    //return logs;

    //    return null;
    //}

    //public CustomStep Get(Guid id)
    //{
    //    var stepEntity = dataContext.CustomSteps.SingleOrDefault(x => x.Id == id && x.CustomStepUsers.Any(x => x.UserId == userId));
    //    var step = mapper.Map<CustomStep>(stepEntity);

    //    return step;
    //}

    //public Guid Create(NewCustomStep step)
    //{ 
    //    var entity = new Entities.CustomStep
    //    {
    //        Id = Guid.NewGuid(),
    //        Name = step.Name,
    //        DisplayName = step.DisplayName,
    //        Description = step.Description,
    //        Icon = step.Icon,
    //        Category = step.Category,
    //        Created = DateTime.UtcNow,
    //        Updated = DateTime.UtcNow,
    //        MoreInfo = string.Empty,
    //        Definition = JsonSerializer.Serialize(new CustomStepDefinition())
    //    };

    //    dataContext.CustomSteps.Add(entity);

    //    var stepUser = new Entities.CustomStepUser
    //    {
    //        CustomStepId = entity.Id,
    //        UserId = userId
    //    };

    //    dataContext.CustomStepUsers.Add(stepUser);

    //    dataContext.SaveChanges();

    //    return entity.Id;
    //}

    //public void Update(Guid id, CustomStep step)
    //{
    //    var entity = dataContext.CustomSteps.SingleOrDefault(x => x.Id == id && x.CustomStepUsers.Any(x => x.UserId == userId));

    //    entity.Name = step.Name;
    //    entity.DisplayName = step.DisplayName;
    //    entity.Description = step.Description;
    //    entity.Icon = step.Icon;
    //    entity.Category = step.Category;
    //    entity.MoreInfo = step.MoreInfo;
    //    entity.VisibleInExplorer = step.VisibleInExplorer;
    //    entity.Updated = DateTime.UtcNow;
    //    entity.Definition = JsonSerializer.Serialize(step.Definition);

    //    dataContext.SaveChanges();
    //}

    //public void Remove(Guid id)
    //{
    //    var entity = dataContext.CustomSteps.SingleOrDefault(x => x.Id == id && x.CustomStepUsers.Any(x => x.UserId == userId));

    //    dataContext.CustomSteps.Remove(entity);

    //    dataContext.SaveChanges();
    //}

    //public bool Exists(string name)
    //{
    //    var exists = dataContext.CustomSteps.Any(x => x.Name == name && x.CustomStepUsers.Any(x => x.UserId == userId));

    //    return exists;
    //}
}
