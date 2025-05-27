using JiraManager.Application.Repositories.Implementations;
using JiraManager.Application.Repositories.Interfaces;
using JiraManager.Application.Services.Implementations;
using JiraManager.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace JiraManager.Application
{
    public static class Startup
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IAccessTokenService, AccessTokenService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IApiService, ApiService>();
            services.AddScoped<IBinanceService, BinanceService>();
            services.AddScoped<IJiraService, JiraService>();

            return services;
        }
    }
}
