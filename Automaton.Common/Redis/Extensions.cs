using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Common.Redis
{
    public static class Extensions
    {
        /// <summary>
        /// Redis configuration
        /// https://jakeydocs.readthedocs.io/en/latest/performance/caching/distributed.html
        /// </summary>
        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;

            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            var options = new RedisOptions();
            configuration.GetSection(nameof(RedisOptions)).Bind(options);
            services.AddSingleton(options);
            
            services.AddDistributedRedisCache(x => 
            {
                x.Configuration = options.ConnectionString;
                x.InstanceName = options.InstanceName;
            });

            return services;
        }
    }
}