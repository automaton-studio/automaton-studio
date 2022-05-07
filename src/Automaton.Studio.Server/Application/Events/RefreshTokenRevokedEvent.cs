using System;
using MediatR;

namespace AuthServer.Core.Events
{
    public class RefreshTokenRevokedEvent : INotification
    {
        public RefreshTokenRevokedEvent(Guid id,Guid userId, string revokedRefreshToken)
        {
            UserId = userId;
            RevokedRefreshToken = revokedRefreshToken;
            Id = id;
        }

        public Guid UserId { get; }
        public Guid Id { get; }
        public string RevokedRefreshToken { get; }
    }
}