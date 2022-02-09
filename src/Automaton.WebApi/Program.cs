using Automaton.WebApi.Config;
using Automaton.WebApi.Interfaces;
using Automaton.WebApi.Middleware;
using Automaton.WebApi.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<FlowsService>();
builder.Services.AddTransient<IDefinitionLoader, DefinitionLoader>();

// Configure services
builder.Services.Configure<AutomatonDatabaseSettings>(builder.Configuration.GetSection("AutomatonDatabase"));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMvc(options =>
{
    options.Filters.Add<RequestObjectFilter>();
    options.EnableEndpointRouting = false;
})
.AddNewtonsoftJson();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddSteps();
builder.Services.AddWorkflow();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
