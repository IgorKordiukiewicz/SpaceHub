using System.Text.Json.Serialization;

namespace Infrastructure.Api.Responses;

public record RocketResponse
{
    [JsonPropertyName("id")]
    public int Id { get; init; }

    [JsonPropertyName("configuration")]
    public RocketConfigResponse Configuration { get; init; }
}
