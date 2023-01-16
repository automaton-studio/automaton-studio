using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Automaton.Studio.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase
    {
        private IMediator _mediator;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        public BaseController()
        {
        }

        public Guid GetUserId()
        {
            var userIdString = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            Guid.TryParse(userIdString, out Guid userId);

            return userId;
        }
    }
}