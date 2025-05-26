using JiraManager.Application.Interfaces;
using JiraManager.Persistence;
using JiraManager.Persistence.Settings;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(config =>
{
    config.ReadFrom.Configuration(builder.Configuration);
    config.Enrich.FromLogContext();
});

builder.Services.Configure<DataBaseSet>(builder.Configuration.GetSection(DataBaseSet.Configuration));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Connection.GetOptionConfiguration(
    builder.Configuration.GetSection(DataBaseSet.Configuration).Get<DataBaseSet>().ConnectionString)));
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

builder.Services.AddControllers();

builder.Services.AddOpenApi();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await Preparation.Initialize(context);
}

app.Run();
