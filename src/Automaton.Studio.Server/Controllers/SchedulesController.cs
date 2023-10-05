using Automaton.Studio.Server.Models;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Automaton.Studio.Server.Controllers
{
    public class SchedulesController : BaseController
    {
        private readonly ScheduleService scheduleService;

        public SchedulesController(ScheduleService scheduleService)
        {
            this.scheduleService = scheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleModel>>> Get(CancellationToken cancellationToken)
        {
            return Ok(await scheduleService.ListAsync(cancellationToken));
        }

        [HttpGet("{flowid}")]
        public async Task<ActionResult<IEnumerable<ScheduleModel>>> Get(Guid flowId, CancellationToken cancellationToken)
        {
            return Ok(await scheduleService.ListAsync(flowId, cancellationToken));
        }

        [HttpPost]
        public async Task<ActionResult> Post(ScheduleModel scheduleFlowDetails, CancellationToken cancellationToken)
        {
            return Ok(await scheduleService.AddAsync(scheduleFlowDetails, cancellationToken));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(Guid id, ScheduleModel scheduleModel, CancellationToken cancellationToken)
        {
            await scheduleService.UpdateAsync(id, scheduleModel, cancellationToken);

            return NoContent();
        }
    }
}
