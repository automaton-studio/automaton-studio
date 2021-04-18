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

        public int Add(Runner runner)
        {
            dbContext.Runners.Add(runner);
            var result = dbContext.SaveChanges();

            return result;
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
