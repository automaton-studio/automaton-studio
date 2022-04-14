using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Events;
using MediatR;

namespace AuthServer.Application.Events.Handlers
{
    public class PasswordChangedEventHandler : INotificationHandler<PasswordChangedEvent>
    {
        public Task Handle(PasswordChangedEvent notification, CancellationToken cancellationToken)
        {
            //TODO
            return Task.CompletedTask;
        }
    }
}