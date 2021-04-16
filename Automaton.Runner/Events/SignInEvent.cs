using MediatR;
using Newtonsoft.Json;

namespace Automaton.Runner.Events
{
    public class SignInEvent : INotification
    {
        public string EmailOrUserName { get; }

        [JsonConstructor]
        public SignInEvent(string emailOrUserName)
        {
            EmailOrUserName = emailOrUserName;
        }
    }
}