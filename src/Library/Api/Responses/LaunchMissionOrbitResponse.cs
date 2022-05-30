using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record LaunchMissionOrbitResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("abbrev")]
        public string Abbrevation { get; init; }
    }
}