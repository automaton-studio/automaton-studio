using AutoMapper;
using Automaton.Studio.Conductor;
using Automaton.Studio.Errors;
using Automaton.Studio.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class DefinitionService : IDefinitionService
    {
        #region Private Members

        private readonly ConfigService configService;
        private HttpClient httpClient;
        private readonly IMapper mapper;
        private readonly ILogger<DefinitionService> logger;

        #endregion

        public DefinitionService
        (
            ConfigService configService, 
            IMapper mapper, 
            HttpClient httpClient,
            ILogger<DefinitionService> logger
        )
        {
            this.logger = logger;
            this.configService = configService;
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            this.mapper = mapper;
        }

        #region Public Methods

        public async Task<IEnumerable<Definition>> List()
        {
            try
            {
                var response = await httpClient.GetAsync($"{configService.ConductorUrl}/api/definition");

                var definitions = await response.Content.ReadAsAsync<IEnumerable<Definition>>();

                return definitions;
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, "Failed to load definitions list");
                
                return new List<Definition>();
            }
        }

        public async Task<Definition> Get(string id)
        {
            var response = await httpClient.GetAsync($"{configService.ConductorUrl}/api/definition/{id}");

            var definition = await response.Content.ReadAsAsync<Definition>();

            return definition;
        }

        public async Task<Definition> Create(string name)
        {
            throw new NotImplementedException();

        }

        public async Task Save(Definition studioFlow)
        {
            throw new NotImplementedException();

        }

        public async Task Delete(Guid flowId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
