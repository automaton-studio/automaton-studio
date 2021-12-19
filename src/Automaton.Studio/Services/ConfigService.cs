using Automaton.Runner.Core.Config;
using Microsoft.Extensions.Configuration;

namespace Automaton.Studio.Services
{
    /// <summary>
    /// Application configuration.
    /// </summary>
    public class ConfigService
    {
        #region Members

        private readonly IConfiguration configuration;

        #endregion

        #region Properties

        public ConductorConfig ConductorConfig { get; private set; } = new ConductorConfig();

        #endregion

        public ConfigService(IConfiguration configuration)
        {
            this.configuration = configuration;

            LoadConductorConfig();
        }

        #region Private Methods

        private void LoadConductorConfig()
        {
            configuration.GetSection(nameof(ConductorConfig)).Bind(ConductorConfig);
        }

        #endregion
    }
}
