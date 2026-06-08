var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.TechMove_Api>("techmove-api");

builder.Build().Run();
