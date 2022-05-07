using System;
using MediatR;

namespace AuthServer.Core.Events
{
    public class PasswordChangedEvent : INotification
    {
        public PasswordChangedEvent(Guid userId, string oldPassword, string newPassword)
        {
            UserId = userId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }

        public Guid UserId { get; }
        public string OldPassword  { get; }
        public string NewPassword { get; }
    }
}