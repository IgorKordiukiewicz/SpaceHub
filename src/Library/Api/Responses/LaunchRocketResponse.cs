using System.Text.Json.Serialization;

namespace Library.Api.Responses
{
    public record LaunchRocketResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; init; }

        [JsonPropertyName("configuration")]
        public LaunchRocketConfigurationResponse Configuration { get; init; }
    }
}