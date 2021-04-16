using Automaton.Runner;
using Automaton.Runner.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuthServer.Application.Handlers
{
    public class SignInEventHandler : INotificationHandler<SignInEvent>
    {
        public Task Handle(SignInEvent notification, CancellationToken cancellationToken)
        {
            var mainWindow = App.Current.MainWindow as MainWindow;
            mainWindow.ShowSetup();

            return Task.CompletedTask;
        }
    }
}