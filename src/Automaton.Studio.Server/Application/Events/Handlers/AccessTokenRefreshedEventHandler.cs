using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Events;
using MediatR;

namespace AuthServer.Application.Events.Handlers
{
    public class AccessTokenRefreshedEventHandler: INotificationHandler<AccessTokenRefreshedEvent>
    {
        public Task Handle(AccessTokenRefreshedEvent notification, CancellationToken cancellationToken)
        { 
            //TODO
            return Task.CompletedTask;
        }
    }
}