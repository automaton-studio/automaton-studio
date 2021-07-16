using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Persistence.EntityFramework.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automaton.Runner.Core.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddElsa(options => options
                .UseEntityFrameworkPersistence(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                    db => db.MigrationsAssembly(typeof(SqlServerElsaContextFactory).Assembly.GetName().Name)))
                .AddConsoleActivities()
                .AddActivity<MessageBox.MessageBox>());


            services.AddSingleton(service => new ConfigService(configuration));
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IRegistrationService, RegisterService>();
            services.AddSingleton<IHubService, HubService>();

            services.AddScoped<IWorkflowService, WorkflowService>();
        }
    }
}
