using Automaton.Elsa.Activities.Dialogs;
using Elsa;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Common.Extensions
{
    public static class ElsaActivitiesExtensions
    {
        /// <summary>
        /// Manually register Elsa activities
        /// </summary>
        /// <param name="options">ElsaOptionsBuilder options reference</param>
        /// <returns>Updated ElsaOptionsBuilder options</returns>
        public static ElsaOptionsBuilder AddElsaActivities(this ElsaOptionsBuilder options)
        {
            options.AddConsoleActivities()
                .AddActivity<MessageBox>();

            return options;
        }
    }
}