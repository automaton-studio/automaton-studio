using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public class LoginService : ILoginService
    {
        private HttpClient httpClient;
        private readonly ConfigService configService;
        private readonly IMapper mapper;
        private readonly ILogger<LoginService> logger;

        public LoginService
        (
            ConfigService configService,
            IMapper mapper,
            HttpClient httpClient,
            ILogger<LoginService> logger
        )
        {
            this.logger = logger;
            this.configService = configService;
            this.httpClient = httpClient;
            this.mapper = mapper;
        }

        public async Task Login(string userName, string password)
        {
            throw new NotImplementedException();
        }
    }
}
