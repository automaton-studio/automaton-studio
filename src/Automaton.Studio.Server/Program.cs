using AuthServer.Core.Domains;
using Automaton.Studio.Server.Areas.Identity;
using Automaton.Studio.Server.Data;
using Automaton.Studio.Server.Hubs;
using Automaton.Studio.Server.Services;
using Common.Authentication;
using Common.EF;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
services.AddScoped<IDataContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

services.AddControllers();
services.AddScoped<FlowsService>();
services.AddScoped<RunnerService>();
services.AddTransient<FlowLoaderService>();
services.AddTransient<UserManagerService>();
services.AddTransient<RoleManagerService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddAutoMapper(Assembly.GetExecutingAssembly());
services.AddMediatR(Assembly.GetExecutingAssembly());
services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

services.AddSteps();
services.AddAutomatonCore();

var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapHub<WorkflowHub>("/api/workflow/hub");
app.MapFallbackToPage("/_Host");

app.Run();
