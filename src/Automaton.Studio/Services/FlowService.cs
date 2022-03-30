using AutoMapper;
using Automaton.Studio.Domain;
using Automaton.Studio.Errors;
using Automaton.Studio.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public class FlowService : IFlowService
    {
        private const string ApplicationJson = "application/json";

        private readonly HttpClient httpClient;
        private readonly ConfigService configService;
        private readonly IFlowConvertService flowConverterService;
        private readonly IMapper mapper;
        private readonly ILogger<FlowService> logger;

        public FlowService
        (
            ConfigService configService,
            IFlowConvertService flowConverterService,
            IMapper mapper,
            HttpClient httpClient,
            ILogger<FlowService> logger
        )
        {
            this.flowConverterService = flowConverterService;
            this.logger = logger;
            this.configService = configService;
            this.httpClient = httpClient;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FlowModel>> List()
        {
            try
            {
                var response = await httpClient.GetAsync($"{configService.FlowsUrl}");

                var flows = await response.Content.ReadAsAsync<IEnumerable<FlowModel>>();

                return flows;
            }
            catch (Exception ex)
            {
                logger.LogError(AppLogEvents.Error, ex, "Failed to load flows list");

                return new List<FlowModel>();
            }
        }

        public async Task<Flow> Load(Guid id)
        {
            var response = await httpClient.GetAsync($"{configService.FlowsUrl}/{id}");
            var flowDto = await response.Content.ReadAsAsync<Dto.Flow>();
            var flow = mapper.Map<Flow>(flowDto);

            return flow;
        }

        public async Task<Flow> Create(Flow flow)
        {
            var flowDto = mapper.Map<Dto.Flow>(flow);
            var response = await httpClient.PostAsJsonAsync(configService.FlowsUrl, flowDto);
            var newFlowDto = await response.Content.ReadAsAsync<Dto.Flow>();
            var newFlow = mapper.Map<Flow>(newFlowDto);
            return newFlow;
        }

        public async Task Update(Flow flow)
        {
            var flowDto = mapper.Map<Dto.Flow>(flow);
            await httpClient.PutAsJsonAsync($"{configService.FlowsUrl}/{flow.Id}", flowDto);
        }

        public async Task Delete(Guid flowId)
        {
            await httpClient.DeleteAsync($"{configService.FlowsUrl}/{flowId}");
        }
    }
}
