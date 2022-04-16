using System;
using MediatR;

namespace AuthServer.Core.Commands
{
    public class RevokeRefreshTokenCommand : IRequest
    {
        public Guid UserId { get; }
        public string Token { get; }

        public RevokeRefreshTokenCommand(Guid userId, string token)
        {
            UserId = userId;
            Token = token;
        }
    }
}