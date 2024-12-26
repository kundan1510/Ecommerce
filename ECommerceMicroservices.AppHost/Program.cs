var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ECommerce_Shared>("ecommerce-shared");

builder.Build().Run();
