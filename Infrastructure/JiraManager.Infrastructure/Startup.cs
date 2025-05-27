using JiraManager.Infrastructure.Services.Binance;
using JiraManager.Infrastructure.Services.JIra;
using Microsoft.Extensions.DependencyInjection;

namespace JiraManager.Infrastructure
{
    public static class Startup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IBinanceApiService, BinanceApiService>();
            services.AddScoped<IJiraApiService, JiraApiService>();

            return services;
        }
    }
}
