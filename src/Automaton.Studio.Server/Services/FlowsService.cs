using AutoMapper;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Models;
using System.Security.Claims;
using System.Text.Json;

namespace Automaton.Studio.Server.Services
{
    public class FlowsService
    {
        private readonly AutomatonDbContext dbContext;
        private readonly ClaimsPrincipal principal;
        private readonly IMapper mapper;
        private readonly string userId;

        public FlowsService(AutomatonDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper)
        {
            this.dbContext = context ?? throw new ArgumentNullException("context");
            principal = httpContextAccessor.HttpContext.User;
            //userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            userId = "13347f27-1f70-497a-b251-5aaacb3fda2e";
            this.mapper = mapper;
        }

        public IEnumerable<Flow> Get()
        {
            var flowIds = dbContext.FlowUsers.AsEnumerable().Where(x => x.UserId == userId).Select(x => x.FlowId);
            var flowEntities = dbContext.Flows.AsEnumerable().Where(x => flowIds.Contains(x.Id));

            var flows = new List<Flow>();

            foreach (var flowEntity in flowEntities)
            {
                var flow = JsonSerializer.Deserialize<Flow>(flowEntity.Body);
                flows.Add(flow);
            }

            return flows;
        }

        public async Task<Flow> GetAsync(Guid id)
        {
            var entity = await dbContext.Flows.FindAsync(id);
            var flow = JsonSerializer.Deserialize<Flow>(entity.Body);

            return flow;
        }

        public async Task<Guid> CreateAsync(Flow flow)
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

            dbContext.Flows.Add(flowEntity);

            var flowUser = new Entities.FlowUser
            {
                FlowId = flowEntity.Id,
                UserId = userId
            };

            dbContext.FlowUsers.Add(flowUser);

            await dbContext.SaveChangesAsync();

            return flowEntity.Id;
        }

        public async Task UpdateAsync(Guid id, Flow flow)
        {
            var existingFlow = await dbContext.Flows.FindAsync(id);

            existingFlow.Name = flow.Name;
            existingFlow.Body = JsonSerializer.Serialize(flow);
            existingFlow.Updated = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(Guid id)
        {
            var flow = await dbContext.Flows.FindAsync(id);

            dbContext.Flows.Remove(flow);

            await dbContext.SaveChangesAsync();
        }
    }
}
