using Automaton.Core.Scripting;
using Automaton.Studio.Server.Areas.Identity;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Entities;
using Automaton.Studio.Server.Hubs;
using Automaton.Studio.Server.Middleware;
using Automaton.Studio.Server.Services;
using Common.Authentication;
using Common.EF;
using Destructurama;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.SystemConsole.Themes;
using System.Data;
using System.Reflection;
using static IronPython.Modules.CTypes;

const string ConnectionStringName = "DefaultConnection";
const string LogEventsSchemaName = "dbo";
const string LogEventsTable = "LogEvents";

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString(ConnectionStringName);

services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

services.AddDatabaseDeveloperPageExceptionFilter();

services.AddIdentity<ApplicationUser, ApplicationRole>()
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
services.AddJwtAuthentication(builder.Configuration);
services.AddHttpClient();

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
services.AddScoped<IDataContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
services.AddControllers();

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();


services.AddTransient<UserNameEnricher>();
services.AddHttpContextAccessor();

var columnOptionsSection = configuration.GetSection("Serilog:ColumnOptions");
var sinkOptionsSection = configuration.GetSection("Serilog:SinkOptions");

var columnOpts = new ColumnOptions
{
    AdditionalColumns = new List<SqlColumn>()
    {
        new SqlColumn { DataType = SqlDbType.NVarChar, ColumnName = "EventType", AllowNull = true }
    }
};

columnOpts.Store.Remove(StandardColumn.Properties);
columnOpts.Store.Add(StandardColumn.LogEvent);

builder.Host.UseSerilog((context, services, config) =>
    config.Destructure.UsingAttributes()
    .Destructure.JsonNetTypes()
    .Enrich.With<EventTypeEnricher>()
    .Enrich.With(services.GetService<UserNameEnricher>())
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Information()
    .WriteTo.MSSqlServer(
        connectionString: ConnectionStringName,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = LogEventsTable,
            SchemaName = LogEventsSchemaName,
            AutoCreateSqlTable = true
        },
        logEventFormatter: new CompactJsonFormatter(),
        sinkOptionsSection: sinkOptionsSection,
        appConfiguration: configuration,
        columnOptions: columnOpts));


services.AddScoped<FlowsService>();
services.AddScoped<RunnerService>();
services.AddScoped<UserContextService>();
services.AddScoped<LogsService>();
services.AddTransient<UserManagerService>();
services.AddTransient<RoleManagerService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddMediatR(Assembly.GetExecutingAssembly());
services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

services.AddScripting();
services.AddSteps();
services.AddAutomatonCore();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapHub<AutomatonHub>("/api/workflow/hub");
app.MapFallbackToPage("/_Host");

app.Run();
