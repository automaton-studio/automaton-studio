using Automaton.Client.Auth.Models;
using Microsoft.Extensions.Configuration;

namespace Automaton.Client.Auth.Services
{
    public class ConfigurationService
    {
        #region Members

        private readonly IConfiguration configuration;
        private readonly AuthConfiguration authConfiguration = new();

        #endregion

        #region Properties

        public string WebApiUrl => authConfiguration.WebApiUrl;
        public string LoginUserUrl => authConfiguration.LoginUserUrl;
        public string RefreshAccessTokenUrl => authConfiguration.RefreshAccessTokenUrl;
        public int RefreshTokenExpirationMinutesCheck => authConfiguration.RefreshTokenExpirationMinutesCheck;

        #endregion

        public ConfigurationService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.configuration.GetSection(nameof(AuthConfiguration)).Bind(authConfiguration);
        }
    }
}
