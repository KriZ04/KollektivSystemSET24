var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.KollektivSystem_ApiService>("apiservice");

builder.AddProject<Projects.KollektivSystem_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
