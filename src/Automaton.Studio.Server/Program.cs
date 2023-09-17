using Automaton.Core.Scripting;
using Automaton.Studio.Server.Areas.Identity;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Automaton.Studio.Server.Middleware;
using Automaton.Studio.Server.Services;
using Common.Authentication;
using Destructurama;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MariaDB.Extensions;
using Serilog.Sinks.MariaDB;
using Serilog.Sinks.MSSqlServer;
using System.Reflection;

var applicationBuilder = WebApplication.CreateBuilder(args);
var configurationManager = applicationBuilder.Configuration;

var configurationBuilder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

var configurationService = new ConfigurationService(configurationBuilder);

var services = applicationBuilder.Services;

services.AddDbContext<ApplicationDbContext>(
    options => _ = configurationService.DatabaseType switch
    {
        ConfigurationService.MySqlDatabaseType => options.UseMySQL(
            configurationManager.GetConnectionString("MySqlConnection"),
            x => x.MigrationsAssembly("Automaton.Studio.Server.MySql.Migrations")),

        _ => throw new Exception($"Unsupported provider: {configurationService.DatabaseType}")
    });

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = configurationService.RequireDigit;
    options.Password.RequireLowercase = configurationService.RequireLowercase;
    options.Password.RequireUppercase = configurationService.RequireUppercase;
    options.Password.RequireNonAlphanumeric = configurationService.RequireNonAlphanumeric;
    options.Password.RequiredLength = configurationService.RequiredLength;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
{
    builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
}));

services.AddMvc(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

services.AddAccessTokenValidator();
services.AddJwtAuthentication(applicationBuilder.Configuration);
services.AddHttpClient();

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
services.AddControllers();

services.AddTransient<UserNameEnricher>();
services.AddHttpContextAccessor();

Serilog.Debugging.SelfLog.Enable(Console.Error);

applicationBuilder.Host.UseSerilog((context, services, config) =>
    config.Destructure.UsingAttributes()
    .ReadFrom.Configuration(configurationBuilder)
    .Destructure.JsonNetTypes()
    .Enrich.With<EventTypeEnricher>()
    .Enrich.With(services.GetService<UserNameEnricher>())
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .WriteTo.Conditional(evt => configurationService.IsDatabaseTypeMsSql(),
        wt => wt.MSSqlServer(
            connectionString: configurationManager.GetConnectionString("MsSqlConnection"),
            appConfiguration: configurationBuilder,
            logEventFormatter: new CompactJsonFormatter(),
            sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs" }))
    .WriteTo.Conditional(evt => configurationService.IsDatabaseTypeMySql(),
        wt => wt.MySQL(
            connectionString: configurationManager.GetConnectionString("MySqlConnection"),
            tableName: "Logs"))
);

services.AddScoped<LogsService>();
services.AddScoped<CustomStepsService>();
services.AddScoped<FlowsService>();
services.AddScoped<RunnerService>();
services.AddScoped<FlowExecutionService>();
services.AddScoped<UserContextService>();
services.AddTransient<UserManagerService>();
services.AddTransient<RoleManagerService>();
services.AddScoped(service => configurationService);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

services.AddScripting();
services.AddAutomatonSteps();
services.AddAutomatonCore();

var app = applicationBuilder.Build();

//app.ApplyMigrations();

app.UseSerilogRequestLogging(
    options =>
    {
        options.MessageTemplate = "{ClientIP} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress);
            // TODO! Need to send UserAgent from Blazor application with each HttpClient request
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
        };
    });

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.MapBlazorHub();
app.MapHub<AutomatonHub>("/api/workflow/hub");
app.MapFallbackToPage("/_Host");

app.Run();
