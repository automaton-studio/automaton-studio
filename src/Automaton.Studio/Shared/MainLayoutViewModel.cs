using Automaton.Client.Auth.Interfaces;
using Automaton.Client.Auth.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Threading.Tasks;

namespace Automaton.Studio.Shared
{
    public class MainLayoutViewModel
    {
        private readonly HttpClient httpClient;
        private readonly IAuthenticationStorage authenticationStorage;
        private readonly AuthenticationStateProvider authStateProvider;

        public MainLayoutViewModel(HttpClient httpClient, IAuthenticationStorage authenticationStorage, AuthenticationStateProvider authStateProvider)
        {
            this.httpClient = httpClient;
            this.authenticationStorage = authenticationStorage;
            this.authStateProvider = authStateProvider;
        }

        public async Task Logout()
        {
            await authenticationStorage.DeleteJsonWebToken();

            ((AuthStateProvider)authStateProvider).NotifyUserLogout();
            httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
