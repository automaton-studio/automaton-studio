using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Automaton.Studio.Server.Services
{
    public class RunnerService
    {
        private const string RunWorkflowMethod = "RunWorkflow";

        private readonly ApplicationDbContext dbContext;
        private readonly IHubContext<WorkflowHub> workflowHubContext;
        private readonly ClaimsPrincipal principal;
        private readonly Guid userId;

        public RunnerService(ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor,
            IHubContext<WorkflowHub> workflowHubContext)
        {
            this.dbContext = dbContext;
            this.workflowHubContext = workflowHubContext;
            principal = httpContextAccessor.HttpContext.User;

            Guid.TryParse(principal.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userIdGuid);
            userId = userIdGuid;
        }

        /// <summary>
        /// Retrieves the full list of runners
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Runner>> List(CancellationToken cancellationToken)
        {
            return await dbContext.Runners.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieve runner by id
        /// </summary>
        /// <param name="id">Runner id</param>
        /// <returns>Runner by incoming id</returns>
        public async Task<Runner> Get(Guid id, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Runners.FindAsync(id, cancellationToken);

            // Because we update Runner's ConnectionId on the fly,
            // when retrieving data we get the cached version of it
            // with previous ConnectionId. There is no need to do the
            // same thing with other entities if they aren't updated
            // in the same way as the Runner entity.

            // Here are some ideas to fix the issue:
            // https://stackoverflow.com/a/51290890/778863
            // http://codethug.com/2016/02/19/Entity-Framework-Cache-Busting/

            // Solution 1. Reload the entity 
            dbContext.Entry(entity).Reload();

            // Solution 2. Detach the entity to remove it from context’s cache.
            // dbContext.Entry(entity).State = EntityState.Detached;
            // entity = dbContext.Runners.Find(id);

            return entity;
        }

        public async Task<int> Create(string name, CancellationToken cancellationToken)
        {
            var runner = new Runner()
            {
                Name = name
            };
            await dbContext.Runners.AddAsync(runner, cancellationToken);

            var runnerUser = new RunnerUser
            {
                RunnerId = runner.Id,
                UserId = userId
            };
            await dbContext.RunnerUsers.AddAsync(runnerUser, cancellationToken);

            var result = await dbContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        public async Task Update(Runner runner, CancellationToken cancellationToken)
        {
            var runnerEntity = dbContext.Runners
                .Include(x => x.RunnerUsers)
                .SingleOrDefault(x => x.Name == runner.Name && x.RunnerUsers.Any(x => x.UserId == userId));

            if (runnerEntity == null)
            {
                throw new ArgumentException("Runner not found");
            }

            // Update connection id
            runnerEntity.ConnectionId = runner.ConnectionId;

            // Mark entity as modified
            dbContext.Entry(runnerEntity).State = EntityState.Modified;

            // Update runner entity
            dbContext.Update(runnerEntity);

            // Save changes
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> Exists(string name, CancellationToken cancellationToken)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = await dbContext.Runners.AnyAsync(x =>
                x.Name.ToLower() == name.ToLower() &&
                x.RunnerUsers.Any(x => x.UserId == userId), 
                cancellationToken: cancellationToken);

            return exists;
        }

        public async Task<bool> DoNotExists(string name, CancellationToken cancellationToken)
        {
            return !await Exists(name, cancellationToken);
        }

        public async Task RunWorkflow(string workflowId, IEnumerable<Guid> runnerIds, CancellationToken cancellationToken)
        {
            foreach (var runnerId in runnerIds)
            {
                var runner = await Get(runnerId, cancellationToken);
                var client = workflowHubContext.Clients.Client(runner.ConnectionId);
                await client.SendAsync(RunWorkflowMethod, workflowId, cancellationToken);
            }
        }
    }
}
