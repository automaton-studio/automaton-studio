using System;
using MediatR;

namespace AuthServer.Core.Events
{
    public class AccessTokenRevokedEvent : INotification
    {
        public Guid UserId { get; }
        public string RevokedAccessToken { get; }
        public AccessTokenRevokedEvent(Guid userId, string revokedAccessToken)
        {
            UserId = userId;
            RevokedAccessToken = revokedAccessToken;
        }
    }
}