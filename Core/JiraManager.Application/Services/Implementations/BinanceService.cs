using JiraManager.Application.Common.Configurations;
using JiraManager.Application.Services.Interfaces;

namespace JiraManager.Application.Services.Implementations
{
    public class BinanceService : IBinanceService
    {
        private readonly IApiService _api;

        public BinanceService(IApiService api) => _api = api;

        public string GetUrl() => _api.GetApiConfiguration(BinanceOption.Url);
    }
}
