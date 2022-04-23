using Automaton.Studio.Models;
using Automaton.Studio.Services.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class RefreshTokenService
    {
        private const string Bearer = "bearer";
        private const string ApplicationJson = "application/json";

        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly ConfigService _configService;
        private readonly LocalStorageService _localStorage;

        public RefreshTokenService(HttpClient client, 
            ConfigService configService,
            LocalStorageService localStorage)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _configService = configService;
            _localStorage = localStorage;
        }

        public async Task<string> RefreshToken()
        {
            var refreshToken = await _localStorage.GetRefreshToken();

            var tokenDto = JsonSerializer.Serialize(new RefreshToken { Token = refreshToken });
            var bodyContent = new StringContent(tokenDto, Encoding.UTF8, ApplicationJson);
            var refreshResult = await _client.PostAsync(_configService.RefreshAccessTokenUrl, bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonWebToken>(refreshContent, _options);

            if (!refreshResult.IsSuccessStatusCode)
            {
                throw new ApplicationException("Something went wrong during the refresh token action");
            }

            await _localStorage.SetAuthAndRefreshTokens(result);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, result.AccessToken);

            return result.AccessToken;
        }
    }
}
