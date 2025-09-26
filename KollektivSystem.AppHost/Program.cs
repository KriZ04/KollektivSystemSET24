using Aspire.Hosting;
using CommunityToolkit.Aspire.Hosting.SqlServer.Extensions;

var builder = DistributedApplication.CreateBuilder(args);


var sql = builder.AddSqlServer("sql")
    .WithAdminer();
                 //.WithLifetime(ContainerLifetime.Persistent);

var db = sql.AddDatabase("database");

var apiService = builder.AddProject<Projects.KollektivSystem_ApiService>("apiservice")
       .WithReference(db)
       .WaitFor(db);

builder.AddProject<Projects.KollektivSystem_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);


builder.Build().Run();
