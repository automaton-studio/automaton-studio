using AuthServer.Core.Dtos;
using AuthServer.Core.Queries;
using AutoMapper;
using Automaton.Studio.Server.Services.Interfaces;
using MediatR;

namespace AuthServer.Application.Queries.Handlers
{
    public class FilterUserQueryHandler : IRequestHandler<FilterUserQuery, IEnumerable<UserDetailInfoDto>>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;

        public FilterUserQueryHandler(IUserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDetailInfoDto>> Handle(FilterUserQuery request,
            CancellationToken cancellationToken)
        {
            var res = await _userManagerService.GetUsers(request.FirstName, request.LastName,
                request.Email, request.PageIndex, request.PageSize
            );

            return _mapper.Map<IEnumerable<UserDetailInfoDto>>(res);
        }
    }
}