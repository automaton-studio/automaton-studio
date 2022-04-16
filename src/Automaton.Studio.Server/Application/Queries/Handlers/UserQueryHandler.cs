using System.Threading;
using System.Threading.Tasks;
using AuthServer.Core.Dtos;
using AuthServer.Core.Queries;
using AuthServer.Core.Services;
using AutoMapper;
using MediatR;

namespace AuthServer.Application.Queries.Handlers
{
    public class UserQueryHandler : IRequestHandler<UserQuery, UserDetailInfoDto>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;
        public UserQueryHandler(IUserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
        }

        public async Task<UserDetailInfoDto> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<UserDetailInfoDto>(await _userManagerService.GetUserById(request.Id));
        }
    }
}
