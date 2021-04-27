using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class RunnerService : IRunnerService
    {
        private readonly AutomatonDbContext dbContext;

        public RunnerService(AutomatonDbContext context)
        {
            this.dbContext = context ?? throw new ArgumentNullException("context");
        }

        public IQueryable<Runner> List()
        {
            return dbContext.Runners;
        }

        public Runner Get(Guid id)
        {           
            var entity = dbContext.Runners.Find(id);

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

        public int Create(Runner runner)
        {
            dbContext.Runners.Add(runner);
            var result = dbContext.SaveChanges();

            return result;
        }

        public async Task Update(Runner runner)
        {
            var runnerEntity = dbContext.Runners.SingleOrDefault(x => x.Name == runner.Name && x.UserId == runner.UserId);

            if (runnerEntity == null)
                throw new ArgumentException("Runner not found");

            // Update connection id
            runnerEntity.ConnectionId = runner.ConnectionId;

            // Mark entity as modified
            dbContext.Entry(runnerEntity).State = EntityState.Modified;

            // Update runner entity
            dbContext.Update(runnerEntity);

            // Save changes
            await dbContext.SaveChangesAsync();
        }

        public bool Exists(Runner runner)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = dbContext.Runners.Any(x =>
                x.Name.ToLower() == runner.Name.ToLower() &&
                x.UserId.ToLower() == runner.UserId.ToLower());

            return exists;
        }
    }
}
