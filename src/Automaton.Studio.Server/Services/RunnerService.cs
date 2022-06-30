using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Automaton.Studio.Server.Services
{
    public class RunnerService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IHubContext<AutomatonHub> automatonHub;
        private readonly Guid userId;

        public RunnerService(ApplicationDbContext dbContext,
            UserContextService userContextService,
            IHubContext<AutomatonHub> automatonHub)
        {
            this.dbContext = dbContext;
            this.automatonHub = automatonHub;
            this.userId = userContextService.GetUserId();
        }

        public async Task<IEnumerable<Runner>> List(CancellationToken cancellationToken)
        {
            var runners = await (from runner in dbContext.Runners
                join runnerUser in dbContext.RunnerUsers
                on runner.Id equals runnerUser.RunnerId
                where runnerUser.UserId == userId
                select runner).ToListAsync(cancellationToken);

            return runners;
        }

        public async Task<IEnumerable<Runner>> List(IEnumerable<Guid> runnerIds, CancellationToken cancellationToken)
        {
            var runners = await (from runner in dbContext.Runners
                join runnerUser in dbContext.RunnerUsers
                on runner.Id equals runnerUser.RunnerId
                where runnerUser.UserId == userId && runnerIds.Contains(runner.Id)
                select runner).ToListAsync(cancellationToken);

            return runners;
        }

        public async Task<Runner> Get(Guid id, CancellationToken cancellationToken)
        {
            var runner =
            (
                from _runner in dbContext.Runners
                join _runnerUser in dbContext.RunnerUsers
                on _runner.Id equals _runnerUser.RunnerId
                where _runner.Id == id && _runnerUser.UserId == userId
                select _runner
            )
            .SingleOrDefault();

            // Because we update Runner's ConnectionId on the fly,
            // when retrieving data we get the cached version of it
            // with previous ConnectionId. There is no need to do the
            // same thing with other entities if they aren't updated
            // in the same way as the Runner entity.

            // Here are some ideas to fix the issue:
            // https://stackoverflow.com/a/51290890/778863
            // http://codethug.com/2016/02/19/Entity-Framework-Cache-Busting/

            // Solution 1. Reload the entity 
            dbContext.Entry(runner).Reload();

            // Solution 2. Detach the entity to remove it from context’s cache.
            // dbContext.Entry(entity).State = EntityState.Detached;
            // entity = dbContext.Runners.Find(id);

            return runner;
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

        public async Task Update(Models.Runner runner, CancellationToken cancellationToken)
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

        public async Task ExecuteFlow(Guid flowId, IEnumerable<Guid> runnerIds, CancellationToken cancellationToken)
        {
            foreach (var runnerId in runnerIds)
            {
                var runner = await Get(runnerId, cancellationToken);
                var client = automatonHub.Clients.Client(runner.ConnectionId);

                await client.SendAsync(AutomatonHubMethods.RunWorkflow, flowId, cancellationToken);
            }
        }
    }
}
