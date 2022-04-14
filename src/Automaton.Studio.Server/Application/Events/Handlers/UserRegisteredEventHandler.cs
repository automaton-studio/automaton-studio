using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Events;
using MediatR;

namespace AuthServer.Application.Events.Handlers
{
    public class UserRegisteredEventHandler : INotificationHandler<UserRegisteredEvent>
    {
        public Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}