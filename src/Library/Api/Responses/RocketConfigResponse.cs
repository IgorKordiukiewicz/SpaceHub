
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

        [JsonPropertyName("image_url")]
        public string? ImageUrl { get; init; }

        [JsonPropertyName("info_url")]
        public string? InfoUrl { get; init; }

        [JsonPropertyName("wiki_url")]
        public string? WikiUrl { get; init; }
    }
}