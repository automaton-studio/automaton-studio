using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Events;
using MediatR;

namespace AuthServer.Application.Events.Handlers
{
    public class RefreshTokenRevokedEventHandler:INotificationHandler<RefreshTokenRevokedEvent>
    {
        public Task Handle(RefreshTokenRevokedEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}