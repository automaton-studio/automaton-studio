using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Events;
using MediatR;

namespace AuthServer.Application.Events.Handlers
{
    public class UserLoggedInEventHandler : INotificationHandler<UserLoggedInEvent>
    {
        public Task Handle(UserLoggedInEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}