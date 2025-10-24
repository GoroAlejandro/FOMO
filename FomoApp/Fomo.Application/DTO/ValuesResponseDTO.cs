using System.Text.Json.Serialization;

namespace Fomo.Application.DTO
{
    public record ValuesResponseDTO
    {
        [JsonPropertyName("meta")]
        public MetaDTO? MetaDTO { get; init; }

        [JsonPropertyName("values")]
        public List<ValuesDTO> Values { get; init; } = new List<ValuesDTO>();

        [JsonPropertyName("status")]
        public  String Status { get; init; } = String.Empty;
    }
}
