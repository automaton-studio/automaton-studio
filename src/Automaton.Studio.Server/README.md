# Automaton Studio Server

## Prerequisites to build and run solution


### Entity Framework CLI installation

The dotnet ef tool is no longer part of the .NET Core SDK
This change allows us to ship dotnet ef as a regular .NET CLI tool that can be installed as either a global or local tool. For example, to be able to manage migrations or scaffold a DbContext, install dotnet ef as a global tool typing the following command:

``dotnet tool install --global dotnet-ef``

## SQL Setup

### Run migration to create/update database

``dotnet ef database update --context AutomatonDbContext``

### Add migration

``dotnet ef migrations add MigrationName --context AutomatonDbContext``

### Swagger URLs

https://localhost/swagger/v1/swagger.json
https://localhost/swagger/index.html










