using JiraManager.Application.DTOs;

namespace JiraManager.Infrastructure.Services.Binance
{
    public interface IBinanceApiService
    {
        Task<CryptoRateDTO> GetRate();
    }
}
