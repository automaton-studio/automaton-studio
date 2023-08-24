# Automaton Studio Server

## Prerequisites to build and run solution


### Entity Framework CLI installation

The dotnet ef tool is no longer part of the .NET Core SDK
This change allows us to ship dotnet ef as a regular .NET CLI tool that can be installed as either a global or local tool. For example, to be able to manage migrations or scaffold a DbContext, install dotnet ef as a global tool typing the following command:

``dotnet tool install --global dotnet-ef``

## Database Providers Setup

### Handle multiple providers migration
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli

### Add migration

When addin migration from Automaton.Server project we get this eror message:

Your target project 'Automaton.Studio.Server' doesn't match your migrations assembly 'Automaton.Studio.Server.MySql.Migrations'. Either change your target project or change your migrations assembly.                                       
Change your migrations assembly by using DbContextOptionsBuilder. E.g. options.UseSqlServer(connection, b => b.MigrationsAssembly("Automaton.Studio.Server")). By default, the migrations assembly is the assembly containing the DbContext. 
Change your target project to the migrations project by using the Package Manager Console's Default project drop-down list, or by executing "dotnet ef" from the directory containing the migrations project.                                

1. Navigate to Automaton.Studio.Server
2. Make sure DatabaseType configuration is "MySql"
 ``"AppConfig": {
    "DatabaseType":  "MySql"
  }``
3. Execute in command line:
``dotnet ef migrations add FlowExecutions --project ../Automaton.Studio.Server.MySql.Migrations/Automaton.Studio.Server.MySql.Migrations.csproj``

Do the same for MsSql migration

1. Navigate to Automaton.Studio.Server
2. Make sure DatabaseType configuration is "MsSql"
 ``"AppConfig": {
    "DatabaseType":  "MsSql"
  }``
3. Execute in command line:
``dotnet ef migrations add FlowExecutions --project ../Automaton.Studio.Server.MsSql.Migrations/Automaton.Studio.Server.MsSql.Migrations.csproj``

### Run migration to create/update database

From console

``dotnet ef database update --context ApplicationDbContext --project Automaton.Studio.Server.MySql.Migrations``

From VS Package Manager Console

Make sure default project us Automaton.Studio.Server

Execute ``Update-Database``

It will apply migrations for the provider specified in app.settings:

``
  "AppConfig": {
    "DatabaseType":  "MySql"
  },
``

Valid entries are:

* MySql
* MsSql



### Swagger URLs

https://localhost:7091/swagger/v1/swagger.json
https://localhost:7091/swagger/index.html










