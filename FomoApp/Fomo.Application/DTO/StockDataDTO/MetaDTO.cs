using System.Text.Json.Serialization;

namespace Fomo.Application.DTO.StockDataDTO
{
    public record MetaDTO
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; init; } = string.Empty;

        [JsonPropertyName("currency")]
        public string Currency { get; init; } = string.Empty;

        [JsonPropertyName("exchange")]
        public string Exchange { get; init; } = string.Empty;
    }
}
