using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fomo.Application.DTO
{
    public record StockResponseDTO
    {
        [JsonPropertyName("data")]
        public List<StockDTO> Data { get; init; } = new List<StockDTO>();

        [JsonPropertyName("status")]
        public string Status { get; init; } = string.Empty;
    }
}
