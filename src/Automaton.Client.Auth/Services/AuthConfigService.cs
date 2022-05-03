using Automaton.Client.Auth.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Client.Auth.Services
{
    public class AuthConfigService
    {
        #region Members

        private readonly IConfiguration configuration;

        #endregion

        #region Properties

        public AuthConfiguration AuthConfig { get; private set; } = new AuthConfiguration();

        public string WebApiUrl => AuthConfig.WebApiUrl;
        public string LoginUserUrl => AuthConfig.LoginUserUrl;
        public string RefreshAccessTokenUrl => AuthConfig.RefreshAccessTokenUrl;
        public int RefreshTokenExpirationMinutesCheck => AuthConfig.RefreshTokenExpirationMinutesCheck;

        #endregion

        public AuthConfigService(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.configuration.GetSection(nameof(AuthConfiguration)).Bind(AuthConfig);
        }
    }
}
