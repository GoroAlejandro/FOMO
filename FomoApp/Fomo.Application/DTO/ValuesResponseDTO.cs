using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
