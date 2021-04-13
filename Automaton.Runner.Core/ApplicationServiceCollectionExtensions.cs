using Automaton.Runner.Core.Auth;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Core
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElsa(options => options
                .UseEntityFrameworkPersistence(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    db => db.MigrationsAssembly(typeof(SqlServerElsaContextFactory).Assembly.GetName().Name)), true)
                .AddConsoleActivities());

            services.AddScoped<IWorkflowService, WorkflowService>();
            services.AddScoped<IAuthService, AuthService>();
        }
    }
}
