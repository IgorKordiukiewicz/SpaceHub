using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record MissionResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("description")]
        public string Description { get; init; }

        [JsonPropertyName("launch_designator")]
        public string? Designator { get; init; }

        [JsonPropertyName("type")]
        public string Type { get; init; }

        [JsonPropertyName("orbit")]
        public OrbitResponse Orbit { get; init; }
    }
}