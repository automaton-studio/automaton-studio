using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Entities;
using Automaton.Studio.Factories;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowDefinitions;
using Elsa.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class FlowService : IFlowService
    {
        #region Private Members

        private readonly AutomatonDbContext dbContext;
        private readonly RunnerService runnerService;

        #endregion

        public FlowService(AutomatonDbContext context, RunnerService workflowService)
        {
            this.dbContext = context ?? throw new ArgumentNullException("context");
            this.runnerService = workflowService;
        }

        #region Public Methods

        /// <summary>
        /// Retrieves the full list of runners
        /// </summary>
        /// <returns></returns>
        public IQueryable<Flow> List()
        {
            return dbContext.Flows;
        }

        /// <summary>
        /// Retrieve runner by id
        /// </summary>
        /// <param name="id">Runner id</param>
        /// <returns>Runner by incoming id</returns>
        public Flow Get(Guid id)
        {
            var entity = dbContext.Flows.Find(id);

            return entity;
        }

        /// <summary>
        /// Adds a new runner to the database
        /// </summary>
        /// <param name="flow">Runner to add</param>
        /// <returns>Result of the runner create operation</returns>
        public int Create(Flow flow)
        {
            dbContext.Flows.Add(flow);
            var result = dbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Update incoming runner into the database
        /// </summary>
        /// <param name="flow">Runner to update information for</param>
        /// <returns>Result of the runner update operation</returns>
        public async Task Update(Flow flow)
        {
            var entity = dbContext.Flows.SingleOrDefault(x => x.Name == flow.Name && x.UserId == flow.UserId);

            if (entity == null)
                throw new ArgumentException("Runner not found");

            // Update entity properties
            entity.Name = flow.Name;

            // Mark entity as modified
            dbContext.Entry(entity).State = EntityState.Modified;

            // Update runner entity
            dbContext.Update(entity);

            // Save changes
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Check if runner exists into the database
        /// </summary>
        /// <param name="flow">Runner to check if exists</param>
        /// <returns>True if runner exists, false if not</returns>
        public bool Exists(Flow flow)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = dbContext.Flows.Any(x =>
                x.Name.ToLower() == flow.Name.ToLower() &&
                x.UserId.ToLower() == flow.UserId.ToLower());

            return exists;
        }

        /// <summary>
        /// Runs a flow on specified runners
        /// </summary>
        /// <param name="flowId">Flow id to run</param>
        /// <param name="runnerIds">Runner ids to run the flow on</param>
        public async Task RunFlow(Guid flowId, IEnumerable<Guid> runnerIds)
        {
            var flow = Get(flowId);

            await runnerService.RunWorkflow(flow.StartupWorkflowId, runnerIds);
        }

        #endregion
    }
}
