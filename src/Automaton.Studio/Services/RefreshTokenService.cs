using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using Blazored.LocalStorage;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly ConfigService _configService;
        private readonly ILocalStorageService _localStorage;

        public RefreshTokenService(HttpClient client, 
            ConfigService configService,
            ILocalStorageService localStorage)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _configService = configService;
            _localStorage = localStorage;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");
            var tokenDto = JsonSerializer.Serialize(new JsonWebToken { AccessToken = token, RefreshToken = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");
            var refreshResult = await _client.PostAsync("api/token/refreshaccesstoken", bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonWebToken>(refreshContent, _options);
            if (!refreshResult.IsSuccessStatusCode)
                throw new ApplicationException("Something went wrong during the refresh token action");
            await _localStorage.SetItemAsync("authToken", result.AccessToken);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
            return result.AccessToken;
        }
    }
}
