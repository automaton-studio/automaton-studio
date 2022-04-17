using AutoMapper;
using Automaton.Studio.Models;
using Blazored.LocalStorage;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Services.Interfaces
{
    public class LocalStorageService : IStorageService
    {
        private const string AuthToken = "authToken";
        private const string RefreshToken = "refreshToken";

        private readonly ILocalStorageService localStorage;

        public LocalStorageService
        (
           ILocalStorageService localStorage
        )
        {
            this.localStorage = localStorage;
        }

        public async Task<string> GetRefreshToken()
        {
            return await localStorage.GetItemAsync<string>(RefreshToken);
        }

        public async Task<string> GetAuthToken()
        {
            return await localStorage.GetItemAsync<string>(AuthToken);
        }

        public async Task SetAuthAndRefreshTokens(JsonWebToken token)
        {
            await localStorage.SetItemAsync("authToken", token.AccessToken);
            await localStorage.SetItemAsync("refreshToken", token.RefreshToken);
        }

        public async Task DeleteAuthAndRefreshTokens()
        {
            await localStorage.RemoveItemAsync("authToken");
            await localStorage.RemoveItemAsync("refreshToken");
        }
    }
}
