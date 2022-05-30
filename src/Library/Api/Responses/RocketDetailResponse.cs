using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record RocketDetailResponse : RocketResponse
    {
        [JsonPropertyName("configuration")]
        public new RocketConfigDetailResponse Configuration { get; init; }
    }
}