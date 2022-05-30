using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record OrbitResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("name")]
        public string Name { get; init; }

        [JsonPropertyName("abbrev")]
        public string Abbrevation { get; init; }
    }
}