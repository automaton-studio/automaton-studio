using Automaton.Studio.Server.Queries;
using Automaton.Studio.Server.Services;
using MediatR;
using Automaton.Studio.Server.Models;

namespace AuthServer.Application.Queries.Handlers
{
    public class FlowLogsQueryHandler : IRequestHandler<FlowLogsQuery, FlowLogsResult>
    {
        private readonly FlowLogsService flowLogsService;

        public FlowLogsQueryHandler(FlowExecutionService flowExecutionService, FlowLogsService logsService)
        {
            this.flowLogsService = logsService;
        }

        public async Task<FlowLogsResult> Handle(FlowLogsQuery request, CancellationToken cancellationToken)
        {
            var flowLogs = await flowLogsService.GetListAsync(request.FlowId, request.StartIndex, request.PageSize);
            var totalExecutions = await flowLogsService.GetTotalAsync(request.FlowId);
            var result = new FlowLogsResult(flowLogs, totalExecutions);

            return result;
        }
    }
}