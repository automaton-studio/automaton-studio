using AutoMapper;
using Automaton.Core.Models;
using Automaton.Studio.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class FlowService
    {
        private readonly HttpClient httpClient;
        private readonly ConfigurationService configService;
        private readonly IMapper mapper;
        private readonly ILogger<FlowService> logger;

        public FlowService
        (
            ConfigurationService configService,
            FlowConvertService flowConverterService,
            IMapper mapper,
            HttpClient httpClient,
            ILogger<FlowService> logger
        )
        {
            this.logger = logger;
            this.configService = configService;
            this.httpClient = httpClient;
            this.mapper = mapper;
        }

        public async Task<StudioFlow> Load(Guid id)
        {
            var response = await httpClient.GetAsync($"{configService.FlowsUrl}/{id}");
            var flowDto = await response.Content.ReadAsAsync<Flow>();

            var flow = mapper.Map<StudioFlow>(flowDto);

            return flow;
        }

        public async Task<StudioFlow> Create(string name)
        {
            var flow = new StudioFlow { Name = name };
            var flowDto = mapper.Map<Flow>(flow);

            var response = await httpClient.PostAsJsonAsync(configService.FlowsUrl, flowDto);
            var result = await response.Content.ReadAsAsync<Flow>();
                
            var newFlow = mapper.Map<StudioFlow>(result);

            return newFlow;
        }

        public async Task Update(StudioFlow flow)
        {
            var flowDto = mapper.Map<Flow>(flow);

            await httpClient.PutAsJsonAsync($"{configService.FlowsUrl}/{flow.Id}", flowDto);
        }

        public async Task Delete(Guid flowId)
        {
            await httpClient.DeleteAsync($"{configService.FlowsUrl}/{flowId}");
        }
    }
}
