using System.Text.Json.Serialization;

namespace Fomo.Application.DTO.StockDataDTO
{
    public record StockResponseDTO
    {
        [JsonPropertyName("data")]
        public List<StockDTO> Data { get; init; } = new List<StockDTO>();

        [JsonPropertyName("status")]
        public string Status { get; init; } = string.Empty;
    }
}
