using System.Text.Json.Serialization;

namespace SpaceHub.Infrastructure.Api.Responses;

public record RocketResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("configuration")]
    public RocketConfigResponse Configuration { get; init; }
}
