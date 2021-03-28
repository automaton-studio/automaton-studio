using Automaton.Runner.Core.Data;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Core
{
    public static class Startup
    {
        public static void ConfigureCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // Elsa Workflows
            services
                .AddElsa(options => options
                    .UseEntityFrameworkPersistence(options =>
                             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                 db => db.MigrationsAssembly(typeof(SqlServerElsaContextFactory).Assembly.GetName().Name)), true)
                    .AddConsoleActivities()
                );

            services.AddScoped<IWorkflowManager, WorkflowManager>();
        }
    }
}
