using Automaton.Common.Extensions;
using Automaton.Runner.Core.Data;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Core.Extensions
{

    public static class ApplicationExtensions
    {
        private const string DatabaseConnection = "DefaultConnection";

        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(DatabaseConnection)));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddElsa(options => options
                .UseEntityFrameworkPersistence(options => 
                options.UseSqlServer(configuration.GetConnectionString(DatabaseConnection),
                    db => db.MigrationsAssembly(typeof(SqlServerElsaContextFactory).Assembly.GetName().Name)), false)
                .AddElsaActivities()
            );

            services.AddSingleton(service => new ConfigService(configuration));
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IRegistrationService, RegisterService>();
            services.AddSingleton<IHubService, HubService>();

            services.AddScoped<IWorkflowService, WorkflowService>();
        }
    }
}
