using System.Text.Json.Serialization;

namespace JiraManager.Infrastructure.Responses.Binance
{
    public class CryptoRateResponse
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; init; }
        [JsonPropertyName("price")]
        public string Price { get; init; }
    }
}
