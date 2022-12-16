using System.Security.Claims;

namespace Automaton.Studio.Server.Services;

public class UserContextService
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public Guid GetUserId()
    {
        var userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        Guid.TryParse(userIdString, out Guid userId);

        return userId;
    }
}
