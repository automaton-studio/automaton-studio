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

            await UpdateRunner();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        #endregion

        #region Public Methods

        public bool Ping(string runnerName)
        {
            return !string.IsNullOrEmpty(runnerName);
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

            return userIdClaim.Value;
        }

        private async Task UpdateRunner()
        {
            var runner = new Runner
            {
                Name = GetRunnerName(),
                UserId = GetUserId(),
                ConnectionId = Context.ConnectionId
            };

            await runnerService.Update(runner);
        }

        #endregion
    }
}
