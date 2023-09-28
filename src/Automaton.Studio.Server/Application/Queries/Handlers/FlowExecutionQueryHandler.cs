using Automaton.Studio.Server.Queries;
using Automaton.Studio.Server.Services;
using MediatR;
using Automaton.Studio.Server.Models;

namespace AuthServer.Application.Queries.Handlers
{
    public class FlowExecutionQueryHandler : IRequestHandler<FlowExecutionQuery, FlowExecutionResult>
    {
        private readonly FlowExecutionService flowExecutionService;
        private readonly FlowLogsService logsService;

        public FlowExecutionQueryHandler(FlowExecutionService flowExecutionService, FlowLogsService logsService)
        {
            this.flowExecutionService = flowExecutionService;
            this.logsService = logsService;
        }

        public async Task<FlowExecutionResult> Handle(FlowExecutionQuery request, CancellationToken cancellationToken)
        {
            var executions = await flowExecutionService.GetListAsync(request.FlowId, request.StartIndex, request.PageSize);
            var totalExecutions = await flowExecutionService.GetTotalAsync(request.FlowId);
            var result = new FlowExecutionResult(executions, totalExecutions);

            return result;
        }
    }
}