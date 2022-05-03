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
    public class FlowsService
    {
        private HttpClient httpClient;
        private readonly ConfigurationService configService;
        private readonly IMapper mapper;
        private readonly ILogger<FlowsService> logger;

        public FlowsService
        (
            ConfigurationService configService,
            IMapper mapper,
            HttpClient httpClient,
            ILogger<FlowsService> logger
        )
        {
            this.logger = logger;
            this.configService = configService;
            this.httpClient = httpClient;
            this.mapper = mapper;
        }

        public async Task<ICollection<FlowDetails>> List()
        {
            try
            {
                var result = await httpClient.GetAsync(configService.FlowsUrl);

                result.EnsureSuccessStatusCode();

                var flows = await result.Content.ReadAsAsync<ICollection<FlowDetails>>();

                return flows;
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, "Failed to load flows list");

                throw ex;
            } 
        }
    }
}
