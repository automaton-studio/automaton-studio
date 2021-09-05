using AutoMapper;
using Automaton.Studio.Core;
using Automaton.Studio.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Automaton.Studio.Services
{
    public class FlowService : IFlowService
    {
        #region Private Members

        private readonly AutomatonDbContext dbContext;
        private readonly ClaimsPrincipal principal;
        private readonly IWorkflowService workflowService;
        private readonly IMapper mapper;
        private readonly string userId;

        #endregion

        public FlowService(AutomatonDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IWorkflowService workflowService,
            IMapper mapper)
        {
            this.workflowService = workflowService;
            this.dbContext = context ?? throw new ArgumentNullException("context");
            principal = httpContextAccessor.HttpContext.User;
            userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            this.mapper = mapper;
        }

        #region Public Methods

        /// <summary>
        /// Retrieves the full list of flows
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StudioFlow> List()
        {
            var flowIds = dbContext.FlowUsers.AsEnumerable().Where(x => x.UserId == userId).Select(x => x.FlowId);
            var flows = dbContext.Flows.AsEnumerable().Where(x => flowIds.Contains(x.Id));
            var studioFlows = mapper.Map<IEnumerable<Flow>, IEnumerable<StudioFlow>>(flows);

            return studioFlows;
        }

        /// <summary>
        /// Retrieve flow by id
        /// </summary>
        /// <param name="id">Flow id</param>
        /// <returns>Flow by id</returns>
        public async Task<StudioFlow> GetAsync(Guid id)
        {
            var flow = dbContext.Flows.Include(x => x.FlowWorkflows).SingleOrDefault(x => x.Id == id);
            var studioFlow = mapper.Map<Flow, StudioFlow>(flow);

            foreach (var flowWorkflow in flow.FlowWorkflows)
            {
                var studioWorkflow = await workflowService.LoadWorkflow(flowWorkflow.WorkflowId);
                studioFlow.Workflows.Add(studioWorkflow);
            }

            // Init active workflow
            studioFlow.ActiveWorkflow = studioFlow.Workflows.SingleOrDefault(x => x.Id == flow.StartupWorkflowId);

            return studioFlow;
        }

        /// <summary>
        /// Adds a new flow to the database
        /// </summary>
        /// <param name="name">Flow name</param>
        /// <returns>Result of the flow create operation</returns>
        public async Task<StudioFlow> Create(string name)
        {
            // Create flow
            var flow = new Flow
            {
                Name = name
            };
            dbContext.Flows.Add(flow);

            // Map flow to user
            var flowUser = new FlowUser
            {
                FlowId = flow.Id,
                UserId = userId
            };
            dbContext.FlowUsers.Add(flowUser);

            await dbContext.SaveChangesAsync();

            var studioFlow = mapper.Map<Flow, StudioFlow>(flow);

            return studioFlow;
        }

        /// <summary>
        /// Update incoming flow into the database
        /// </summary>
        /// <param name="flow">Flow to update information for</param>
        /// <returns>Result of the flow update operation</returns>
        public async Task Update(StudioFlow studioFlow)
        {
            var flowUser = await dbContext.FlowUsers.FindAsync(studioFlow.Id, userId);
            var flow = await dbContext.Flows.FindAsync(flowUser.FlowId);

            foreach (var studioWorkflow in studioFlow.Workflows)
            {
                await workflowService.SaveWorkflow(studioWorkflow);

                var flowWorkflow = await dbContext.FlowWorkflows.FindAsync(flow.Id, studioWorkflow.Id);

                if (flowWorkflow == null)
                {
                    flowWorkflow = new FlowWorkflow
                    {
                        WorkflowId = studioWorkflow.Id,
                        FlowId = flow.Id
                    };

                    dbContext.FlowWorkflows.Add(flowWorkflow);
                }
            }

            // Update Flow with Studio Flow details
            mapper.Map(studioFlow, flow);

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes flow from the database
        /// </summary>
        /// <param name="flowId">Flow Id to delete</param>
        /// <returns>Result of the flow delete operation</returns>
        public async Task Delete(Guid flowId)
        {
            var flow = await dbContext.Flows.FindAsync(flowId);

            dbContext.Flows.Remove(flow);

            await dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Check if flow exists into the database
        /// </summary>
        /// <param name="name">Flow name</param>
        /// <returns>True if flow exists, false if not</returns>
        public bool Exists(string name)
        {
            // Note: OrdinalCase comparison not working with this version of LinQ
            var exists = dbContext.Flows.Any(x => x.Name.ToLower() == name.ToLower() && x.FlowUsers.Any(x => x.UserId == userId));

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
