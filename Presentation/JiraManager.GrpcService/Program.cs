WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

WebApplication app = builder.Build();

app.Run();
