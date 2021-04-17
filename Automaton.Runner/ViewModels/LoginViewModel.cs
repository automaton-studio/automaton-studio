using Automaton.Runner.Core;
using Automaton.Runner.Events;
using Automaton.Runner.Services;
using MediatR;
using System;
using System.Threading.Tasks;

namespace Automaton.Runner.ViewModels
{
    public class LoginViewModel
    {
        private readonly IAppConfigurationService configService;
        private readonly IAuthService authService;
        private readonly IMediator mediator;

        public LoginViewModel(
            IAppConfigurationService configService,
            IAuthService authService,
            IMediator mediator)
        {
            this.configService = configService;
            this.authService = authService;
            this.mediator = mediator;
        }

        public async Task Login(string username, string password)
        {
            try
            {
                var studioConfig = configService.GetStudioConfig();

                var userCredentials = new UserCredentials
                {
                    UserName = username,
                    Password = password
                };

                var token = await authService.GetToken(userCredentials, studioConfig.TokenApiUrl);

                await mediator.Publish(new SignInEvent(username, token));
            }
            catch (Exception ex)
            {
            }
        }
    }
}
