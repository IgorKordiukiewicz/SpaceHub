using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library.Api.Responses
{
    public record RocketsResponse : MultiElementResponse
    {
        [JsonPropertyName("results")]
        public IEnumerable<RocketConfigResponse> Rockets { get; init; }
    }
}
