using Automaton.Studio.Server.Queries;
using Automaton.Studio.Server.Services;
using MediatR;
using static Automaton.Studio.Server.Models.ApiModels;

namespace AuthServer.Application.Queries.Handlers
{
    public class FilterFlowExecutionQueryHandler : IRequestHandler<FilterFlowExecutionQuery, FlowExecutionResult>
    {
        private readonly FlowExecutionService flowExecutionService;
        private readonly LogsService logsService;

        public FilterFlowExecutionQueryHandler(FlowExecutionService flowExecutionService, LogsService logsService)
        {
            this.flowExecutionService = flowExecutionService;
            this.logsService = logsService;
        }

        public async Task<FlowExecutionResult> Handle(FilterFlowExecutionQuery request, CancellationToken cancellationToken)
        {
            var executions = await flowExecutionService.GetForFlow(request.FlowId, request.StartIndex, request.PageSize);
            var totalExecutions = await flowExecutionService.GetTotal(request.FlowId);
            var result = new FlowExecutionResult(executions, totalExecutions);

            return result;
        }
    }
}