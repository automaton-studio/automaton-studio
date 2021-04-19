using Automaton.Studio.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Automaton.Studio.Hubs
{
    [Authorize(JwtBearerDefaults.AuthenticationScheme)]
    public class WorkflowHub : Hub
    {
        #region Constants

        private const string UserIdClaim = "uid";
        private const string RunnerNameHeader = "RunnerName";

        #endregion

        #region Members

        private readonly IRunnerService runnerService;

        #endregion

        #region Constructors

        public WorkflowHub(IRunnerService runnerService)
        {
            this.runnerService = runnerService;
        }

        #endregion

        #region Overrides

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("WelcomeRunner", $"Welcome {GetRunnerName()}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region Public Methods

        public bool RegisterRunner(string runnerName)
        {
            var runner = new Runner
            {
                Name = runnerName,
                ConnectionId = Context.ConnectionId,
                UserId = GetUserId()
            };

            var result = runnerService.Add(runner);

            return result > 0;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves connected runner name from request headers
        /// </summary>
        /// <returns>Connected runner name</returns>
        private string GetRunnerName()
        {
            var httpCtx = Context.GetHttpContext();
            var runnerName = httpCtx.Request.Headers[RunnerNameHeader].ToString();

            return runnerName;
        }

        /// <summary>
        /// Retrieves connected user Id
        /// </summary>
        /// <returns>Connected user Id</returns>
        private string GetUserId()
        {
            var userIdClaim = Context.User.Claims.SingleOrDefault(x => x.Type == UserIdClaim);

            if (userIdClaim is null)
                throw new ArgumentNullException("userId");

            var userId = userIdClaim.Value;

            return userIdClaim.Value;
        }

        #endregion
    }
}
