using AutoMapper;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Models;

namespace Automaton.Studio.Server.Services;

public class LogsService
{
    private readonly ApplicationDbContext dataContext;
    private readonly IMapper mapper;
    private readonly Guid userId;

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
    }

    //public IEnumerable<Log> List()
    //{
    //    var logEntities =
    //    (
    //        from _log in dataContext.Logs
    //        join _logUser in dataContext.Users
    //        on _log.UserId equals _logUser.Id
    //        where _logUser.Id == userId
    //        select _log
    //    );

    //    var logs = mapper.Map<IEnumerable<Log>>(logEntities);

    //    return logs;
    //}

    //public Log Get(Guid id)
    //{
    //    var logEntity = 
    //    (
    //        from _log in dataContext.Logs
    //        join _logUser in dataContext.Users
    //        on _log.UserId equals _logUser.Id
    //        where _log.Id == id && _logUser.Id == userId
    //        select _log
    //    )
    //    .SingleOrDefault();

    //    var log = mapper.Map<Log>(logEntity);

    //    return log;
    //}

    //public Guid Create(Log log)
    //{
    //    var logEntity = new Entities.Log
    //    {
    //        UserId = userId,
    //        EventName = log.EventName,
    //        CreatedDate = log.CreatedDate,
    //        ExceptionMessage = log.ExceptionMessage,
    //        LogLevel = log.LogLevel,
    //        Source = log.Source,
    //        StackTrace = log.StackTrace
    //    };

    //    dataContext.Logs.Add(logEntity);

    //    dataContext.SaveChanges();

    //    return logEntity.Id;
    //}

    //public void Update(Guid id, Log log)
    //{
    //    var logEntity =
    //    (
    //        from _log in dataContext.Logs
    //        join _logUser in dataContext.Users
    //        on _log.UserId equals _logUser.Id
    //        where _log.Id == id && _logUser.Id == userId
    //        select _log
    //    )
    //    .SingleOrDefault();

    //    logEntity.EventName = log.EventName;
    //    logEntity.CreatedDate = log.CreatedDate;
    //    logEntity.ExceptionMessage = log.ExceptionMessage;
    //    logEntity.LogLevel = log.LogLevel;
    //    logEntity.Source = log.Source;
    //    logEntity.StackTrace = log.StackTrace;

    //    dataContext.SaveChanges();
    //}

    //public void Remove(Guid id)
    //{
    //    var log =
    //    (
    //        from _log in dataContext.Logs
    //        join _logUser in dataContext.Users
    //        on _log.UserId equals _logUser.Id
    //        where _log.Id == id && _logUser.Id == userId
    //        select _log
    //    )
    //    .SingleOrDefault();

    //    dataContext.Logs.Remove(log);

    //    dataContext.SaveChanges();
    //}
}
