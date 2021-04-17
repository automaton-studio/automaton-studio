using Automaton.Runner.Core;
using MediatR;
using Newtonsoft.Json;

namespace Automaton.Runner.Events
{
    public class SignInEvent : INotification
    {
        public string EmailOrUserName { get; }
        public JsonWebToken Token { get; }

        [JsonConstructor]
        public SignInEvent(string emailOrUserName, JsonWebToken token)
        {
            EmailOrUserName = emailOrUserName;
            Token = token;
        }
    }
}