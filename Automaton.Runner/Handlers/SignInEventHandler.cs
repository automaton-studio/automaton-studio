using Automaton.Runner;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Events;
using Automaton.Runner.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AuthServer.Application.Handlers
{
    public class SignInEventHandler : INotificationHandler<SignInEvent>
    {
        IHubService hubService;
        IAppConfigurationService appConfiguration;

        public SignInEventHandler(IAppConfigurationService appConfiguration, IHubService hubService)
        {
            this.appConfiguration = appConfiguration;
            this.hubService = hubService;
        }

        public async Task Handle(SignInEvent signInEvent, CancellationToken cancellationToken)
        {
            var mainWindow = App.Current.MainWindow as MainWindow;

            if (appConfiguration.IsRunnerRegistered())
            {
                await hubService.Connect(signInEvent.Token, appConfiguration.GetRunnerName());

                mainWindow.ShowDashboardControl();
            }
            else
            {
                mainWindow.ShowRegistrationControl();
            }
        }
    }
}