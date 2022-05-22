using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace Automaton.Studio.Server.Hubs
{
    public class WorkflowHub : Hub
    {
        #region Constants

        private const string UserIdClaim = "uid";
        private const string RunnerNameHeader = "RunnerName";

        #endregion

        #region Members

        private readonly RunnerService runnerService;

        #endregion

        #region Constructors

        public WorkflowHub(RunnerService runnerService)
        {
            this.runnerService = runnerService;
        }

        #endregion

        #region Overrides

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("WelcomeRunner", $"Welcome {GetRunnerName()}");

            await UpdateRunnerConnection();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
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

        private string GetUserId()
        {
            var userIdClaim = Context.User.Claims.SingleOrDefault(x => x.Type == UserIdClaim);

            if (userIdClaim is null)
                throw new ArgumentNullException("userId");

            return userIdClaim.Value;
        }

        private async Task UpdateRunnerConnection()
        {
            var runner = new Runner
            {
                Name = GetRunnerName(),
                ConnectionId = Context.ConnectionId
            };

            var userId = GetUserId();

            await runnerService.Update(runner, CancellationToken.None);
        }

        #endregion
    }
}
