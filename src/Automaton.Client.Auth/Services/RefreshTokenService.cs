using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Automaton.Client.Auth.Services
{
    public class RefreshTokenService
    {
        private const string Bearer = "bearer";
        private const string ApplicationJson = "application/json";

        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _options;
        private readonly ConfigurationService _configService;
        private readonly IAuthenticationStorage _localStorage;

        public RefreshTokenService(HttpClient client,
            ConfigurationService configService,
            IAuthenticationStorage localStorage)
        {
            _client = client;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _configService = configService;
            _localStorage = localStorage;
        }

        public async Task<string> RefreshToken()
        {
            var refreshToken = await _localStorage.GetRefreshToken();
            var jsonToken = JsonSerializer.Serialize(new { Token = refreshToken });
            var bodyContent = new StringContent(jsonToken, Encoding.UTF8, ApplicationJson);
            var refreshResult = await _client.PostAsync(_configService.RefreshAccessTokenUrl, bodyContent);
            var refreshContent = await refreshResult.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<JsonWebToken>(refreshContent, _options);

            if (!refreshResult.IsSuccessStatusCode)
            {
                throw new ApplicationException("Something went wrong during the refresh token action");
            }

            await _localStorage.SetJsonWebToken(result);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(Bearer, result.AccessToken);

            return result.AccessToken;
        }
    }
}
