using AuthServer.Core.Dtos;
using AuthServer.Core.Queries;
using AutoMapper;
using Automaton.Studio.Server.Services;
using MediatR;

namespace AuthServer.Application.Queries.Handlers
{
    public class UserQueryHandler : IRequestHandler<UserQuery, UserDetails>
    {
        private readonly UserManagerService _userManagerService;
        private readonly IMapper _mapper;
        public UserQueryHandler(UserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
        }

        public async Task<UserDetails> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<UserDetails>(await _userManagerService.GetUserById(request.Id));
        }
    }
}
