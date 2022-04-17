using Common.Authentication;
using MediatR;

namespace AuthServer.Core.Commands
{
    public class RefreshAccessTokenCommand : IRequest<JsonWebToken>
    {
        public string Token { get; set; }
    }
}