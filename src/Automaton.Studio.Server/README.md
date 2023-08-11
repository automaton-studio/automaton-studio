# Automaton Studio Server

## Prerequisites to build and run solution


### Entity Framework CLI installation

The dotnet ef tool is no longer part of the .NET Core SDK
This change allows us to ship dotnet ef as a regular .NET CLI tool that can be installed as either a global or local tool. For example, to be able to manage migrations or scaffold a DbContext, install dotnet ef as a global tool typing the following command:

``dotnet tool install --global dotnet-ef``

## Database Providers Setup

### Handle multiple providers migration
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/projects?tabs=dotnet-core-cli

### Run migration to create/update database

From console

``dotnet ef database update --context ApplicationDbContext --project Automaton.Studio.Server.MySql.Migrations``

From VS Package Manager Console

``Update-Database``

It will aply migrations for the provider specified in app.settings:

``
  "AppConfig": {
    "DatabaseType":  "MySql"
  },
``

Valid entries are:

* MySql
* MsSql

### Add migration

``dotnet ef migrations add MigrationName --project Automaton.Studio.Server.MsSql.Migrations.csproj``
``dotnet ef migrations add MigrationName --project Automaton.Studio.Server.MySql.Migrations.csproj``

### Swagger URLs

https://localhost:7091/swagger/v1/swagger.json
https://localhost:7091/swagger/index.html










