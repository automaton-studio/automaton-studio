using MediatR;
using Newtonsoft.Json;

namespace AuthServer.Core.Events
{
    public class UserRegisteredEvent : INotification 
    {
        public string UserName { get; }
        public string Email { get; }
        public string FirstName { get;  }
        public string LastName { get; }

        [JsonConstructor]
        public UserRegisteredEvent(string userName, string email, string firstName, string lastName)
        {
            Email = email;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
