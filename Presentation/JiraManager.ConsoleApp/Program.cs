using JiraManager.Application.Interfaces;
using JiraManager.Persistence;
using JiraManager.Persistence.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, loggerConfiguration) => loggerConfiguration
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext());

builder.Services.Configure<DataBaseSet>(builder.Configuration.GetSection(DataBaseSet.Configuration));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Connection.GetOptionConfiguration(
    builder.Configuration.GetSection(DataBaseSet.Configuration).Get<DataBaseSet>().ConnectionString)));
builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

using IHost host = builder.Build();

using (IServiceScope scope = host.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await Preparation.Initialize(context);
}

await host.RunAsync();