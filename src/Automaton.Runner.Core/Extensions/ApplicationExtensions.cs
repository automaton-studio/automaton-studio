using Automaton.Runner.Core.Data;
using Automaton.Runner.Core.Services;
using Automaton.Runner.Services;
using Automaton.Studio.AuthProviders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

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

            var configService = new ConfigService(configuration);
            services.AddSingleton<AuthenticationStateProvider, AuthStateProvider>();
            services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri(configService.StudioConfig.WebApiUrl) });

            services.AddSingleton(service => new ConfigService(configuration));
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IRegistrationService, RegisterService>();
            services.AddSingleton<IHubService, HubService>();

            services.AddScoped<WorkflowService>();
        }
    }
}
