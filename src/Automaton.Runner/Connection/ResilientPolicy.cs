using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using System;

namespace Automaton.Runner.Connection
{
    public class ResilientPolicy : IConnectionPolicy
    {
        private readonly AsyncRetryPolicy retryPolicy;
        private readonly AsyncCircuitBreakerPolicy circuitBreakerPolicy;
        private readonly AsyncPolicyWrap resilientPolicy;

        public ResilientPolicy(int retryAfterSeconds, int exceptionsAllowedBeforeBreaking, TimeSpan durationOfBreak)
        {
            retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetryForeverAsync(retryAttempt => TimeSpan.FromSeconds(retryAfterSeconds),
                (exception, retry, time) =>
                {
                    // Do nothing. Keep it for debugging purpose to check when the policy is triggered
                });


            circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(exceptionsAllowedBeforeBreaking, durationOfBreak);

            resilientPolicy = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
        }

        public AsyncPolicy GetPolicy()
        {
            return resilientPolicy;
        }
    }
}
