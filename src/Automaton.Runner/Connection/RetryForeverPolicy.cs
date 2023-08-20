using Polly;
using Polly.Retry;
using System;

namespace Automaton.Runner.Connection
{
    public class RetryForeverPolicy : IConnectionPolicy
    {
        private readonly AsyncRetryPolicy retryPolicy;

        public RetryForeverPolicy()
        {
            retryPolicy = Policy
                .Handle<Exception>()
                // Continuously increase the time to retry as 2 power of retry attempt (2 ^ retryAttempt)
                .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, retry, time) =>
                {
                    // Do nothing. Keep it for debugging purpose to check when the policy is triggered
                });
        }

        public AsyncPolicy GetPolicy()
        {
            return retryPolicy;
        }
    }
}
