using Automaton.Runner.Core;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Automaton.Runner.Controls
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public event EventHandler<AuthTokenArgs> LoginSuccessful;

        protected virtual void OnLoginSuccessful(AuthTokenArgs e)
        {
            var handler = LoginSuccessful;
            handler?.Invoke(this, e);
        }

        public LoginControl()
        {
            InitializeComponent();
        }

        private async void LoginClick(object sender, RoutedEventArgs e)
        {
            var userDetails = new SignInUserDetails
            {
                UserName = UsernameBox.Text,
                Password = PasswordBox.Password
            };

            var objAsJson = JsonConvert.SerializeObject(userDetails);
            var content = new StringContent(objAsJson, Encoding.UTF8, "application/json");
            var _httpClient = new HttpClient();
            var response = await _httpClient.PostAsync("https://localhost:5001/api/token", content);

            var authTokenJson = await response.Content.ReadAsStringAsync();
            var authToken = JsonConvert.DeserializeObject<JsonWebToken>(authTokenJson);

            var tokenArgs = new AuthTokenArgs
            {
                AuthToken = authToken
            };

            OnLoginSuccessful(tokenArgs);
        }
    }
}
