using Common.Authentication;
using MediatR;

namespace Automaton.Studio.Server.Core.Commands
{
    public class RefreshAccessTokenCommand : IRequest<JsonWebToken>
    {
        public string Token { get; set; }
    }
}