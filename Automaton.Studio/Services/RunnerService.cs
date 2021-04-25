using System;
using System.Linq;

namespace Automaton.Studio.Services
{
    public class RunnerService : IRunnerService
    {
        private readonly AutomatonDbContext dbContext;

        public RunnerService(AutomatonDbContext context)
        {
            this.dbContext = context ?? throw new ArgumentNullException("context");
        }

        public int Create(Runner runner)
        {
            dbContext.Runners.Add(runner);
            var result = dbContext.SaveChanges();

            return result;
        }

        public bool Exists(Runner runner)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = dbContext.Runners.Any(x =>
                x.Name.ToLower() == runner.Name.ToLower() &&
                x.UserId.ToLower() == runner.UserId.ToLower());

            return exists;
        }

        public IQueryable<Runner> Get()
        {
            return dbContext.Runners;
        }

        public Runner Get(Guid id)
        {
            return dbContext.Set<Runner>().Find(id);
        }
    }
}
