using AutoMapper;
using Automaton.Studio.Domain;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
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

        public async Task<Flow> Load(Guid id)
        {
            var response = await httpClient.GetAsync($"{configService.FlowsUrl}/{id}");
            var flowDto = await response.Content.ReadAsAsync<Dto.Flow>();

            var flow = mapper.Map<Flow>(flowDto);

            return flow;
        }

        public async Task<Flow> Create(string name)
        {
            var flow = new Flow { Name = name };
            var flowDto = mapper.Map<Dto.Flow>(flow);

            var response = await httpClient.PostAsJsonAsync(configService.FlowsUrl, flowDto);
            var result = await response.Content.ReadAsAsync<Dto.Flow>();
                
            var newFlow = mapper.Map<Flow>(result);

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
