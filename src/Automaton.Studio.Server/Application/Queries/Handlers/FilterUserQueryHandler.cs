using AuthServer.Core.Dtos;
using AuthServer.Core.Queries;
using AutoMapper;
using Automaton.Studio.Server.Services;
using MediatR;

namespace AuthServer.Application.Queries.Handlers
{
    public class FilterUserQueryHandler : IRequestHandler<FilterUserQuery, IEnumerable<UserDetails>>
    {
        private readonly UserManagerService _userManagerService;
        private readonly IMapper _mapper;

        public FilterUserQueryHandler(UserManagerService userManagerService, IMapper mapper)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDetails>> Handle(FilterUserQuery request,
            CancellationToken cancellationToken)
        {
            var res = await _userManagerService.GetUsers(request.FirstName, request.LastName,
                request.Email, request.PageIndex, request.PageSize
            );

            return _mapper.Map<IEnumerable<UserDetails>>(res);
        }
    }
}