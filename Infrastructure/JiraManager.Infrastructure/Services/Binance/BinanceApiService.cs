using JiraManager.Application.DTOs;
using JiraManager.Application.Services.Interfaces;
using JiraManager.Infrastructure.Responses.Binance;
using System.Text.Json;

namespace JiraManager.Infrastructure.Services.Binance
{
    public class BinanceApiService : IBinanceApiService
    {
        private readonly IBinanceService _binance;

        private readonly IHttpClientFactory _clientFactory;

        public BinanceApiService(IBinanceService binance, IHttpClientFactory clientFactory)
        {
            _binance = binance;
            _clientFactory = clientFactory;
        }

        public async Task<CryptoRateDTO> GetRate()
        {
            string destination = $"{_binance.GetUrl()}/ticker/price";

            using HttpClient client = _clientFactory.CreateClient();

            HttpResponseMessage response = await client.GetAsync(destination);

            if (!response.IsSuccessStatusCode)
            {
                string errorMessage = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Error: {errorMessage}");
            }
            string content = await response.Content.ReadAsStringAsync();

            IEnumerable<CryptoRateResponse> rateResponse = JsonSerializer.Deserialize<IEnumerable<CryptoRateResponse>>(content);

            return new CryptoRateDTO()
            {
                XRPUSDT = rateResponse.Where(x => x.Symbol == nameof(CryptoRateDTO.XRPUSDT)).Select(x => decimal.Parse(x.Price)).FirstOrDefault(),
                DASHUSDT = rateResponse.Where(x => x.Symbol == nameof(CryptoRateDTO.DASHUSDT)).Select(x => decimal.Parse(x.Price)).FirstOrDefault(),
                BTCUSDT = rateResponse.Where(x => x.Symbol == nameof(CryptoRateDTO.BTCUSDT)).Select(x => decimal.Parse(x.Price)).FirstOrDefault(),
                XMRUSDT = rateResponse.Where(x => x.Symbol == nameof(CryptoRateDTO.XMRUSDT)).Select(x => decimal.Parse(x.Price)).FirstOrDefault(),
            };
        }
    }
}
