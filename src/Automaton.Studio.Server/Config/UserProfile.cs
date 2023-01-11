using Automaton.Studio.Server.Core.Commands;
using AuthServer.Core.Events;
using AutoMapper;

namespace AuthServer.Core.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterUserCommand, UserRegisteredEvent>()
                .ForAllMembers(x => x.UseDestinationValue());
        }
    }
}