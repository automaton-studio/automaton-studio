using AuthServer.Core.Queries;
using AutoMapper;
using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using MediatR;

namespace AuthServer.Application.Queries.Handlers
{
    public class RunnerQueryHandler : IRequestHandler<RunnerQuery, IEnumerable<Runner>>
    {
        private readonly IMapper mapper;
        private readonly RunnerService runnerService;

        public RunnerQueryHandler(RunnerService runnerService, IMapper mapper)
        {
            this.mapper = mapper;
            this.runnerService = runnerService;
        }

        public async Task<IEnumerable<Runner>> Handle(RunnerQuery request, CancellationToken cancellationToken)
        {
            var runnerEntities = request.RunnerIds.Any() ? 
                await runnerService.List(request.RunnerIds, cancellationToken) :
                await runnerService.List(cancellationToken);

            var runners = mapper.Map<IEnumerable<Runner>>(runnerEntities);

            return runners;
        }
    }
}
