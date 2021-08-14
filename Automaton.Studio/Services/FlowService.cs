using Automaton.Studio.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Automaton.Studio.Models;
using Automaton.Studio.Core;

namespace Automaton.Studio.Services
{
    public class FlowService : IFlowService
    {
        #region Private Members

        private readonly AutomatonDbContext dbContext;
        private readonly ClaimsPrincipal principal;
        private readonly IWorkflowService workflowService;
        private readonly string userId;

        #endregion

        public FlowService(AutomatonDbContext context, 
            IHttpContextAccessor httpContextAccessor,
            IWorkflowService workflowService)
        {
            this.workflowService = workflowService;
            this.dbContext = context ?? throw new ArgumentNullException("context");
            principal = httpContextAccessor.HttpContext.User;
            userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        #region Public Methods

        /// <summary>
        /// Retrieves the full list of flows
        /// </summary>
        /// <returns></returns>
        public IQueryable<Flow> List()
        {
            return dbContext.Flows;
        }

        /// <summary>
        /// Retrieve flow by id
        /// </summary>
        /// <param name="id">Flow id</param>
        /// <returns>Flow by id</returns>
        public Flow Get(Guid id)
        {
            var entity = dbContext.Flows.Find(id);

            return entity;
        }

        /// <summary>
        /// Retrieve flow by name
        /// </summary>
        /// <param name="id">Flow name</param>
        /// <returns>Flow by name</returns>
        public Flow Get(string name)
        {
            var flow = dbContext.Flows.SingleOrDefault(x => x.Name.ToLower() == name.ToLower());

            return flow;
        }

        /// <summary>
        /// Adds a new flow to the database
        /// </summary>
        /// <param name="flow">Flow to add</param>
        /// <returns>Result of the flow create operation</returns>
        public async Task<int> Create(Flow flow)
        {
            throw new NotImplementedException();

            //// Update flow UserId and add flow
            //flow.UserId = userId;
            //dbContext.Flows.Add(flow);

            //// Create default workflow
            //var defaultWorkflow = new StudioWorkflow();
            //await workflowService.SaveWorkflow(defaultWorkflow);

            //// Set flow's StartupWorkflowId
            //flow.StartupWorkflowId = defaultWorkflow.Id;

            //// Map workflow to parent flow
            //var flowWorkflow = new FlowWorkflow
            //{
            //    WorkflowId = defaultWorkflow.Id,
            //    FlowId = flow.Id
            //};
            //dbContext.FlowWorkflows.Add(flowWorkflow);

            //var result = dbContext.SaveChanges();

            //return result;
        }

        /// <summary>
        /// Update incoming flow into the database
        /// </summary>
        /// <param name="flow">Flow to update information for</param>
        /// <returns>Result of the flow update operation</returns>
        public async Task Update(Flow flow)
        {
            var entity = dbContext.Flows.SingleOrDefault(x => x.Name == flow.Name && flow.FlowUsers.Any(x => x.UserId == userId));

            if (entity == null)
                throw new ArgumentException("Flow not found");

            // Update entity properties
            entity.Name = flow.Name;

            // Mark entity as modified
            dbContext.Entry(entity).State = EntityState.Modified;

            // Update flow entity
            dbContext.Update(entity);

            // Save changes
            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes flow from the database
        /// </summary>
        /// <param name="flowId">Flow Id to delete</param>
        /// <returns>Result of the flow delete operation</returns>
        public int Delete(Guid flowId)
        {
            var flow = Get(flowId);

            dbContext.Flows.Remove(flow);
            var result = dbContext.SaveChanges();

            return result;
        }

        /// <summary>
        /// Check if flow exists into the database
        /// </summary>
        /// <param name="flow">Flow to check if exists</param>
        /// <returns>True if flow exists, false if not</returns>
        public bool Exists(Flow flow)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = dbContext.Flows.Any(x =>
                x.Name.ToLower() == flow.Name.ToLower() &&
                flow.FlowUsers.Any(x => x.UserId == userId));

            return exists;
        }

        /// <summary>
        /// Check if flow exists into the database
        /// </summary>
        /// <param name="name">Flow name</param>
        /// <returns>True if flow exists, false if not</returns>
        public bool Exists(string name)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = dbContext.Flows.Any(x =>x.Name.ToLower() == name.ToLower() && x.FlowUsers.Any(x => x.UserId == userId));

            return exists;
        }

        /// <summary>
        /// Check if flow name is unique for current user
        /// </summary>
        /// <param name="name">Flow name</param>
        /// <returns>True if flow does not exists, false if exists</returns>
        public bool IsUnique(string name)
        {
            return !Exists(name);
        }

        #endregion
    }
}
