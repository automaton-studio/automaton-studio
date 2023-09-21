using Microsoft.AspNetCore.Http;
using Serilog.Core;
using System.Security.Claims;

public class UserNameEnricher : ILogEventEnricher
{
    readonly IHttpContextAccessor _httpContextAccessor;

    public UserNameEnricher(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void Enrich(Serilog.Events.LogEvent logEvent, ILogEventPropertyFactory factory)
    {
        if (!(_httpContextAccessor.HttpContext?.User.Identity.IsAuthenticated ?? false))
        {
            return;
        }

        var user = _httpContextAccessor.HttpContext.User;
        var userName = user.Identity.Name;
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var userNameProperty = factory.CreateProperty("UserName", userName);
        logEvent.AddOrUpdateProperty(userNameProperty);

        var userIdProperty = factory.CreateProperty("UserId", userId);
        logEvent.AddOrUpdateProperty(userIdProperty);
    }
}