using System;
using MediatR;

namespace AuthServer.Core.Events
{
    public class AccessTokenRefreshedEvent : INotification
    {
        public AccessTokenRefreshedEvent(Guid userId, string accessToken, string refreshToken)
        {
            UserId = userId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }

        public Guid UserId { get; }
        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}