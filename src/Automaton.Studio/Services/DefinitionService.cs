using AutoMapper;
using Automaton.Studio.Conductor;
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

        #endregion

        public DefinitionService(ConfigService configService, IMapper mapper, HttpClient httpClient)
        {
            this.configService = configService;
            this.httpClient = httpClient;
            this.httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            this.mapper = mapper;
        }

        #region Public Methods

        /// <summary>
        /// Retrieves the full list of flows
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Definition>> List()
        {
            try
            {
                var response = await httpClient.GetAsync($"{configService.ConductorConfig.ConductorUrl}/api/definition");

                var definitions = await response.Content.ReadAsAsync<IEnumerable<Definition>>();

                return definitions;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        /// <summary>
        /// Retrieve flow by id
        /// </summary>
        /// <param name="id">Flow id</param>
        /// <returns>Flow by id</returns>
        public async Task<Definition> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a new flow to the database
        /// </summary>
        /// <param name="name">Flow name</param>
        /// <returns>Result of the flow create operation</returns>
        public async Task<Definition> Create(string name)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Update incoming flow into the database
        /// </summary>
        /// <param name="flow">Flow to update information for</param>
        /// <returns>Result of the flow update operation</returns>
        public async Task Update(Definition studioFlow)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// Deletes flow from the database
        /// </summary>
        /// <param name="flowId">Flow Id to delete</param>
        /// <returns>Result of the flow delete operation</returns>
        public async Task Delete(Guid flowId)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
