using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Fomo.Application.DTO
{
    public record ValuesDTO
    {
        [JsonPropertyName("datetime")]
        public string Datetime { get; init; } = string.Empty;

        [JsonPropertyName("open")]
        public string Open { get; init; } = string.Empty;

        [JsonPropertyName("high")]
        public string High { get; init; } = string.Empty;

        [JsonPropertyName("low")]
        public string Low { get; init; } = string.Empty;

        [JsonPropertyName("close")] 
        public string Close { get; init; } = string.Empty;

        [JsonPropertyName("volume")]
        public string Volume { get; init; } = string.Empty;
    }
}
