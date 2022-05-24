using AuthServer.Core.Queries;
using AutoMapper;
using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using MediatR;

namespace AuthServer.Application.Queries.Handlers
{
    public class RunnerQueryHandler : IRequestHandler<RunnerQuery, IEnumerable<Runner>>
    {
        private readonly RunnerService runnerService;
        private readonly IMapper mapper;
        public RunnerQueryHandler(RunnerService runnerService, IMapper mapper)
        {
            this.runnerService = runnerService;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<Runner>> Handle(RunnerQuery request, CancellationToken cancellationToken)
        {
            var runnerEntities = await runnerService.List(cancellationToken);

            var runners = mapper.Map<IEnumerable<Runner>>(runnerEntities);

            return runners;
        }
    }
}
