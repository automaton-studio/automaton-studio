using System;
using MediatR;

namespace AuthServer.Core.Events
{
    public class UserLoggedInEvent : INotification
    {
        public UserLoggedInEvent(Guid id)
        {
            Id = id;
        }

        public Guid Id { get;  }
    }
}