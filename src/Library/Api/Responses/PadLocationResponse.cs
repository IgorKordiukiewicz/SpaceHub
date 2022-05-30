using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record PadLocationResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("url")]
        public string Url { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("country_code")]
        public string CountryCode { get; init; }

        [JsonPropertyName("map_image")]
        public string MapImageUrl { get; init; }

        [JsonPropertyName("total_launch_count")]
        public int TotalLaunches { get; init; }

        [JsonPropertyName("total_landing_count")]
        public int TotalLandings { get; init; }
    }
}