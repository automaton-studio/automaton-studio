using Serilog.Core;

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

        // Access the name of the logged-in user
        var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
        var userNameProperty = factory.CreateProperty("UserName", userName);
        logEvent.AddPropertyIfAbsent(userNameProperty);
    }
}