using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Events;
using MediatR;

namespace AuthServer.Application.Events.Handlers
{
    public class AccessTokenRevokedEventHandler: INotificationHandler<AccessTokenRevokedEvent>
    {
        public Task Handle(AccessTokenRevokedEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}