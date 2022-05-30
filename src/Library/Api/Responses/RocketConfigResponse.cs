
using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record RocketConfigResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("family")]
        public string Family { get; init; }

        [JsonPropertyName("full_name")]
        public string FullName { get; init; }

        [JsonPropertyName("variant")]
        public string Variant { get; init; }
    }
}