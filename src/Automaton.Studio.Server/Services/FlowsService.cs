using Automaton.Core.Models;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.Json;

namespace Automaton.Studio.Server.Services
{
    public class FlowsService
    {
        private readonly ApplicationDbContext dataContext;
        private readonly RunnerService runnerService;
        private readonly IHubContext<WorkflowHub> workflowHubContext;

        private readonly Guid userId;

        public FlowsService(ApplicationDbContext dataContext,
            RunnerService runnerService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.dataContext = dataContext;
            this.runnerService = runnerService;

            var userIdString = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(userIdString, out Guid userIdGuid);
            userId = userIdGuid;
        }

        public IEnumerable<Entities.Flow> List()
        {
            var flows = from flow in dataContext.Flows
                join flowUser in dataContext.FlowUsers
                on flow.Id equals flowUser.FlowId
                where flowUser.UserId == userId
                select flow;

            return flows;
        }

        public Flow Get(Guid id)
        {
            var flow = 
            (
                from _flow in dataContext.Flows
                join _flowUser in dataContext.FlowUsers
                on _flow.Id equals _flowUser.FlowId
                where _flow.Id == id && _flowUser.UserId == userId
                select DeserializeFlow(_flow.Body)
            )
            .SingleOrDefault();

            return flow;
        }

        public Guid Create(Flow flow)
        {
            flow.Id = Guid.NewGuid();

            var flowEntity = new Entities.Flow
            {
                Id = flow.Id,
                Name = flow.Name,
                Body = JsonSerializer.Serialize(flow),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            dataContext.Flows.Add(flowEntity);

            var flowUser = new Entities.FlowUser
            {
                FlowId = flowEntity.Id,
                UserId = userId
            };

            dataContext.FlowUsers.Add(flowUser);

            dataContext.SaveChanges();

            return flowEntity.Id;
        }

        public void Update(Guid id, Flow flow)
        {
            var flowEntity = GetFlowEntity(id);
            flowEntity.Name = flow.Name;
            flowEntity.Body = JsonSerializer.Serialize(flow);
            flowEntity.Updated = DateTime.UtcNow;

            dataContext.SaveChanges();
        }

        public void Remove(Guid id)
        {
            var flow = GetFlowEntity(id);

            dataContext.Flows.Remove(flow);

            dataContext.SaveChanges();
        }

        private static Flow DeserializeFlow(string flowBody)
        {
            var flow = JsonSerializer.Deserialize<Flow>(flowBody);

            return flow;
        }

        private Entities.Flow GetFlowEntity(Guid id)
        {
            var flow = 
            (
                from _flow in dataContext.Flows
                join _flowUser in dataContext.FlowUsers
                on _flow.Id equals _flowUser.FlowId
                where _flow.Id == id && _flowUser.UserId == userId
                select _flow
            )
            .SingleOrDefault();

            return flow;
        }
    }
}
