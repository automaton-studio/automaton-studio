using AutoMapper;
using Automaton.Studio.Errors;
using Automaton.Studio.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public class FlowsService : IFlowsService
    {
        private HttpClient httpClient;
        private readonly ConfigService configService;
        private readonly IMapper mapper;
        private readonly ILogger<DefinitionService> logger;

        public FlowsService
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

        public async Task<IEnumerable<FlowModel>> List()
        {
            try
            {
                var response = await httpClient.GetAsync($"{configService.ConductorUrl}/api/flow");

                var flows = await response.Content.ReadAsAsync<IEnumerable<FlowModel>>();

                return flows;
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, "Failed to load flows list");

                return new List<FlowModel>();
            }
        }
    }
}
