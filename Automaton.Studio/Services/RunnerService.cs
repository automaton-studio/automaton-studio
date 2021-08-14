using Automaton.Studio.Entities;
using Automaton.Studio.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class RunnerService : IRunnerService
    {
        #region Private Members

        private readonly AutomatonDbContext dbContext;
        private readonly IHubContext<WorkflowHub> workflowHubContext;
        private readonly ClaimsPrincipal principal;
        private readonly string userId;

        #endregion

        public RunnerService(AutomatonDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IHubContext<WorkflowHub> workflowHubContext)
        {
            this.dbContext = context ?? throw new ArgumentNullException("context");
            this.workflowHubContext = workflowHubContext;
            principal = httpContextAccessor.HttpContext.User;
            userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        #region Public Methods

        /// <summary>
        /// Retrieves the full list of runners
        /// </summary>
        /// <returns></returns>
        public IQueryable<Runner> List()
        {
            return dbContext.Runners;
        }

        /// <summary>
        /// Retrieve runner by id
        /// </summary>
        /// <param name="id">Runner id</param>
        /// <returns>Runner by incoming id</returns>
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

        /// <summary>
        /// Adds a new runner to the database
        /// </summary>
        /// <param name="runner">Runner to add</param>
        /// <returns>Result of the runner create operation</returns>
        public int Create(string name)
        {
            var runner = new Runner()
            {
                Name = name
            };
            dbContext.Runners.Add(runner);

            var runnerUser = new RunnerUser
            {
                RunnerId = runner.Id,
                UserId = userId
            };
            dbContext.RunnerUsers.Add(runnerUser);

            var result = dbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Update incoming runner into the database
        /// </summary>
        /// <param name="runner">Runner to update information for</param>
        /// <returns>Result of the runner update operation</returns>
        public async Task Update(Runner runner)
        {
            var runnerEntity = dbContext.Runners.SingleOrDefault(x => x.Name == runner.Name && runner.RunnerUsers.Any(x => x.UserId == userId));

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

        /// <summary>
        /// Check if runner exists into the database
        /// </summary>
        /// <param name="runner">Runner to check if exists</param>
        /// <returns>True if runner exists, false if not</returns>
        public bool Exists(string name)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = dbContext.Runners.Any(x =>
                x.Name.ToLower() == name.ToLower() &&
                x.RunnerUsers.Any(x => x.UserId == userId));

            return exists;
        }

        /// <summary>
        /// Runs a workflow on specified runners
        /// </summary>
        /// <param name="workflowId">Workflow id to run</param>
        /// <param name="runnerIds">RUnner ids to run the workflow on</param>
        public async Task RunWorkflow(string workflowId, IEnumerable<Guid> runnerIds)
        {
            foreach (var runnerId in runnerIds)
            {
                var runner = Get(runnerId);
                var client = workflowHubContext.Clients.Client(runner.ConnectionId);
                await client.SendAsync("RunWorkflow", workflowId);
            }
        }

        #endregion
    }
}
