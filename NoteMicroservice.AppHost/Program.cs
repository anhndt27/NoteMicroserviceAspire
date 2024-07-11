var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("cache");

var gateWay = builder.AddProject<Projects.NoteMicroservice_MinimalApi>("gateway")
    .WithExternalHttpEndpoints();

var note = builder.AddProject<Projects.NoteMicroservice_Note_API>("note-api");

var staticFile = builder.AddProject<Projects.WebAppStaticFile>("staticfile-api");

var identity = builder.AddProject<Projects.NoteMicroservice_Identity>("identity-api");

builder.AddNpmApp("react", "../NoteMicroservice.React")
    .WithReference(gateWay)
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
